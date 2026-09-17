using System.Net;
using System.Text.Json;
using Account.Api.Contracts;
using Account.Application.Exceptions;
using Account.Domain.Exceptions;

namespace Account.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;
    private readonly IHostEnvironment environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        this.next = next;
        this.logger = logger;
        this.environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await this.next(context);
        }
        catch (Exception ex)
        {
            var (statusCode, code) = MapException(ex);

            if (statusCode == HttpStatusCode.InternalServerError)
            {
                this.logger.LogError(ex, "Unhandled exception processing {Method} {Path}", context.Request.Method, context.Request.Path);
            }
            else
            {
                this.logger.LogWarning(ex, "Handled exception processing {Method} {Path}", context.Request.Method, context.Request.Path);
            }

            var message = statusCode == HttpStatusCode.InternalServerError && !this.environment.IsDevelopment()
                ? "An unexpected error occurred."
                : ex.Message;

            var response = GatewayResponse<object>.Fail(new GatewayError
            {
                Code = code,
                Message = message,
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, SerializerOptions));
        }
    }

    private static (HttpStatusCode StatusCode, string Code) MapException(Exception ex) => ex switch
    {
        AccountNotFoundException or MovementNotFoundException => (HttpStatusCode.NotFound, "not_found"),
        DuplicateAccountNumberException => (HttpStatusCode.Conflict, "conflict"),
        SaldoNoDisponibleException or CuentaInactivaException => (HttpStatusCode.Conflict, "conflict"),
        ArgumentException => (HttpStatusCode.BadRequest, "invalid_argument"),
        _ => (HttpStatusCode.InternalServerError, "internal_error"),
    };
}
