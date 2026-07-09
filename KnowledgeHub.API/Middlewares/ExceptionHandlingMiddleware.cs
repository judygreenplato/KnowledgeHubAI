using System.Text.Json;
using KnowledgeHub.Application.Exceptions;

namespace KnowledgeHub.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync( HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(
                context,
                ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType =
            "application/json";

        int statusCode =
            StatusCodes.Status500InternalServerError;

        switch (exception)
        {
            case NotFoundException:
                statusCode =
                    StatusCodes.Status404NotFound;
                break;

            case ForbiddenException:
                statusCode =
                    StatusCodes.Status403Forbidden;
                break;

            case ValidationException:
                statusCode =
                    StatusCodes.Status400BadRequest;
                break;
        }

        context.Response.StatusCode =
            statusCode;

        var response = new
        {
            Message = exception.Message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}