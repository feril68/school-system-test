public class ApiLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiLoggingMiddleware> _logger;

    public ApiLoggingMiddleware(RequestDelegate next, ILogger<ApiLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string traceId = context.TraceIdentifier;

        context.Request.EnableBuffering();

        string requestBody = await ReadRequestBodyAsync(context);

        _logger.LogInformation("TraceId: {TraceId} | API Hit: {Method} {Path}, Query: {Query}, Body: {Body}, Headers: {Headers}",
            traceId,
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString,
            requestBody,
            string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}: {h.Value}")));

        context.Request.Body.Position = 0;

        await _next(context);
    }

    private async Task<string> ReadRequestBodyAsync(HttpContext context)
    {
        if (context.Request.Body == null || !context.Request.Body.CanSeek)
        {
            return string.Empty;
        }

        using (var reader = new StreamReader(
            context.Request.Body,
            encoding: System.Text.Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            bufferSize: 1024,
            leaveOpen: true))
        {
            var body = await reader.ReadToEndAsync();
            return body;
        }
    }
}
