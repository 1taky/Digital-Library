using System.Text.Json;
using DigitalLibrary.BLL.Exceptions;

namespace DigitalLibrary.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException exception)
        {
            await WriteErrorResponseAsync(
                context,
                exception.StatusCode,
                exception.Message);
        }
        catch (Exception)
        {
            await WriteErrorResponseAsync(
                context,
                500,
                "Внутрішня помилка сервера.");
        }
    }

    private static async Task WriteErrorResponseAsync(
        HttpContext context,
        int statusCode,
        string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new
        {
            statusCode,
            message
        };

        string json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}