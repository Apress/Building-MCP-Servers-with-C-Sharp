using System.Security.Cryptography;
using System.Text;

namespace EnterpriseServer.Services;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;

    public ApiKeyMiddleware(
        RequestDelegate next,
        IConfiguration config)
    {
        _next = next;
        _config = config;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        var path = context.Request.Path.Value ?? "";

        // Skip auth for health checks
        if (path == "/health")
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("X-API-Key", out var providedKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API key is required.");

            return;
        }

        var expectedKey = _config["ApiKey"] ?? "";
        
        var providedBytes = Encoding.UTF8.GetBytes(providedKey.ToString());
        var expectedBytes = Encoding.UTF8.GetBytes(expectedKey);

        if (!CryptographicOperations.FixedTimeEquals(providedBytes, expectedBytes))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Invalid API key.");

            return;
        }

        await _next(context);
    }
}
