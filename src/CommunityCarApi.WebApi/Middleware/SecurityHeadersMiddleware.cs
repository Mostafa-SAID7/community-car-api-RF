using Microsoft.Extensions.Primitives;

namespace CommunityCarApi.WebApi.Middleware;

/// <summary>
/// Middleware to add security headers to HTTP responses
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Remove server header
        context.Response.Headers.Remove("Server");
        context.Response.Headers.Remove("X-Powered-By");

        // Add security headers
        context.Response.Headers.Append("X-Content-Type-Options", new StringValues("nosniff"));
        context.Response.Headers.Append("X-Frame-Options", new StringValues("DENY"));
        context.Response.Headers.Append("X-XSS-Protection", new StringValues("1; mode=block"));
        context.Response.Headers.Append("Referrer-Policy", new StringValues("strict-origin-when-cross-origin"));
        context.Response.Headers.Append("Permissions-Policy", new StringValues("geolocation=(), microphone=(), camera=()"));
        
        // Content Security Policy
        context.Response.Headers.Append("Content-Security-Policy", new StringValues(
            "default-src 'self'; " +
            "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdnjs.cloudflare.com https://cdn.jsdelivr.net; " +
            "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdnjs.cloudflare.com; " +
            "font-src 'self' https://fonts.gstatic.com https://cdnjs.cloudflare.com; " +
            "img-src 'self' data: https:; " +
            "connect-src 'self'; " +
            "frame-ancestors 'none';"
        ));

        await _next(context);
    }
}
