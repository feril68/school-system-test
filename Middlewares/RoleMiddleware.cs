using System.Security.Claims;

public class RoleMiddleware
{
    private readonly RequestDelegate _next;

    public RoleMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole == null)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { message = "Access denied: no role assigned" });
                return;
            }
        }

        await _next(context);
    }
}
