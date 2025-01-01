using Microsoft.AspNetCore.Mvc.Infrastructure;
using SchoolSystem.Exceptions;
using System.Text.Json;

namespace SchoolSystem.Middlewares
{
    public class CustomExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlingMiddleware> _logger;
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public CustomExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<CustomExceptionHandlingMiddleware> logger,
            ProblemDetailsFactory problemDetailsFactory)
        {
            _next = next;
            _logger = logger;
            _problemDetailsFactory = problemDetailsFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                switch (exception)
                {
                    case CustomException customException:
                        _logger.LogError(
                            "TraceId: {TraceId} | [Expected Error] Message: {Message}, Title: {Title}, StatusCode: {StatusCode}, " +
                            "Thrown by {CallerMemberName} in {CallerFilePath}:line {CallerLineNumber}",
                            context.TraceIdentifier,
                            customException.Message,
                            customException.Title,
                            customException.StatusCode,
                            customException.CallerMemberName,
                            customException.CallerFilePath,
                            customException.CallerLineNumber
                        );
                        await HandleExceptionAsync(context, exception);
                        break;

                    default:
                        _logger.LogError(exception, "TraceId: {TraceId} | [Unhandled Exception]  Internal Server Error", context.TraceIdentifier);
                        await HandleExceptionAsync(context, exception);
                        break;
                }
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            int statusCode;
            string title;

            switch (exception)
            {
                case CustomException customException:
                    statusCode = customException.StatusCode;
                    title = customException.Title;
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    title = "Internal Server Error";
                    break;
            }

            var problemDetails = _problemDetailsFactory.CreateProblemDetails(
                httpContext,
                statusCode: statusCode,
                title: title,
                detail: exception.Message
            );

            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(problemDetails);
            await httpContext.Response.WriteAsync(json);
        }
    }
}
