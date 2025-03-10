using System.Text;
using WebApplicationAdvertisingPlatforms.Exceptions;

namespace WebApplicationAdvertisingPlatforms.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment)
{
    private readonly RequestDelegate _next = next;
    private readonly IWebHostEnvironment _environment = environment;

    public async Task InvokeAsync(HttpContext context)
    {
        try { await _next(context); }
        catch (Exception ex)
        {
            HandleException(context, ex);

            if (_environment.IsDevelopment())
                await WriteExceptionToResponse(context, ex);
        }
    }

    private async Task WriteExceptionToResponse(HttpContext context, Exception exception)
    {
        var errorMessage = $"{exception.Message}\n\n\n{exception.StackTrace}";
        var errorMessageBytes = Encoding.UTF8.GetBytes(errorMessage);

        context.Response.ContentType = "text/plain";
        await context.Response.Body.WriteAsync(errorMessageBytes);
    }

    private void HandleException(HttpContext context, Exception exception)
    {
        // TODO 9: Можно описать какую-то хитрую логику обработки и логгирования ошибок.

        // Пока что идея в том, чтобы поставить правильный статус код ответу в зависимости от вызванного исключения.
        if (exception is BadRequestException
            || exception is ArgumentException)
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        else if (exception is NotFoundException) context.Response.StatusCode = StatusCodes.Status404NotFound;
        else context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    }
}
