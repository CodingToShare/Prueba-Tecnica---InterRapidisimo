using System.Net;
using System.Text.Json;
using FluentValidation;

namespace StudentRegistration.Api.Middleware;

/// <summary>
/// Middleware global para el manejo de excepciones.
/// Intercepta las excepciones no controladas y devuelve respuestas HTTP consistentes.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error no controlado durante la solicitud: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        
        var response = new ErrorResponse
        {
            Message = ex.Message
        };

        switch (ex)
        {
            case KeyNotFoundException:
                // Recurso no encontrado -> 404
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;

            case InvalidOperationException:
                // Error de regla de negocio o conflicto -> 409
                // Nota: A veces se usa 400, pero 409 es común para conflictos de estado
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                break;

            case UnauthorizedAccessException:
                // No autorizado -> 401 (o 403 dependiendo del caso)
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;

            case ValidationException validationEx:
                // Error de validación de FluentValidation -> 400
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = "Errores de validación";
                response.Errors = validationEx.Errors.Select(e => new { Field = e.PropertyName, Error = e.ErrorMessage });
                break;

            case ArgumentException:
                // Argumento inválido -> 400
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            default:
                // Error interno del servidor -> 500
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = _env.IsDevelopment() 
                    ? ex.Message 
                    : "Ha ocurrido un error interno en el servidor.";
                break;
        }

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, jsonOptions);

        await context.Response.WriteAsync(json);
    }
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public object? Errors { get; set; }
}
