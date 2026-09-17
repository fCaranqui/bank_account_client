using System.Reflection;
using System.Text.Json.Serialization;
using Client.Api.Contracts;
using Client.Api.Middleware;
using Client.Application.UseCases;
using Client.Domain.Repository;
using Client.Infrastructure.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/client-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(entry => entry.Value?.Errors.Count > 0)
            .SelectMany(entry => entry.Value!.Errors.Select(error => new GatewayError
            {
                Code = "invalid_argument",
                Message = string.IsNullOrEmpty(error.ErrorMessage) ? "Valor inválido." : error.ErrorMessage,
            }));

        return new BadRequestObjectResult(GatewayResponse<object>.Fail(errors));
    };
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Client API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddDbContext<ClientDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ClientDb")));

builder.Services.AddScoped<IClienteRepository, PgClienteRepository>();

builder.Services.AddScoped<CreateClientUseCase>();
builder.Services.AddScoped<GetClientByIdUseCase>();
builder.Services.AddScoped<GetAllClientsUseCase>();
builder.Services.AddScoped<UpdateClientUseCase>();
builder.Services.AddScoped<DeleteClientUseCase>();
builder.Services.AddScoped<ActivateClientUseCase>();
builder.Services.AddScoped<DeactivateClientUseCase>();
builder.Services.AddScoped<UpdatePasswordUseCase>();

const string corsPolicy = "ClientCorsPolicy";
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
        policy.WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<ClientDbContext>().Database.Migrate();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSerilogRequestLogging();

app.UseCors(corsPolicy);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/// <summary>
/// Exposes the generated Program class to WebApplicationFactory-based integration tests.
/// </summary>
public partial class Program
{
}
