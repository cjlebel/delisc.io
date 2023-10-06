using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Configuration;

namespace Deliscio.Core.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;

    private const string API_KEY_NAME = "x-api-key";

    public ApiKeyMiddleware(IConfiguration config, RequestDelegate next)
    {
        _config = config;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(API_KEY_NAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Api Key was not provided by client. ");
            return;
        }

        // This will work too. Just not sure which is better.
        //var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();

        //TODO: Use OptionSettings?
        var apiKey = _config.GetValue<string>("ApiKey") ?? string.Empty;

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("No Api Key was found on server. ");
            return;
        }

        if (!apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Thou Shall Not Pass!");
            return;
        }

        await _next(context);
    }
}