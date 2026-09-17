using System.Reflection;
using System.Text.Json.Serialization;
using Account.Api.Contracts;
using Account.Api.Middleware;
using Account.Application.UseCases;
using Account.Domain.Repository;
using Account.Infrastructure.Db;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/account-.log", rollingInterval: RollingInterval.Day)
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
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Account API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddDbContext<AccountDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AccountDb")));

builder.Services.AddScoped<ICuentaRepository, PgCuentaRepository>();
builder.Services.AddScoped<IClientReplicaRepository, ClientReplicaRepository>();

if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddMassTransit(x =>
    {
        x.AddConsumer<Account.Infrastructure.Events.ClientCreatedEventConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
            {
                h.Username(builder.Configuration["RabbitMq:Username"]!);
                h.Password(builder.Configuration["RabbitMq:Password"]!);
            });

            cfg.Message<Account.Application.Events.ClientCreatedEvent>(m => m.SetEntityName("client-created"));

            // Client and Account each keep their own copy of ClientCreatedEvent in different
            // namespaces (this repo's standing "duplicated, not shared" convention for message
            // contracts). MassTransit's default envelope embeds the publisher's fully-qualified
            // CLR type name and only routes to a consumer whose registered type matches exactly,
            // so two structurally-identical-but-differently-named types never match by default and
            // the message gets dead-lettered. Raw JSON with AnyMessageType deserializes by message
            // shape against this endpoint's registered consumer type instead of checking the
            // publisher's type name, which is what makes the two copies interoperate.
            cfg.UseRawJsonSerializer(RawSerializerOptions.AnyMessageType);

            cfg.ReceiveEndpoint("account-client-created", e =>
            {
                e.ConfigureConsumer<Account.Infrastructure.Events.ClientCreatedEventConsumer>(context);
            });
        });
    });
}

builder.Services.AddScoped<CreateAccountUseCase>();
builder.Services.AddScoped<GetAccountByIdUseCase>();
builder.Services.AddScoped<GetAllAccountsUseCase>();
builder.Services.AddScoped<GetAccountsByClientIdUseCase>();
builder.Services.AddScoped<UpdateAccountUseCase>();
builder.Services.AddScoped<RegisterMovementUseCase>();
builder.Services.AddScoped<GetMovementByIdUseCase>();
builder.Services.AddScoped<GetMovementsByAccountIdUseCase>();
builder.Services.AddScoped<GenerateAccountStatementReportUseCase>();

const string corsPolicy = "AccountCorsPolicy";
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
    scope.ServiceProvider.GetRequiredService<AccountDbContext>().Database.Migrate();
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
