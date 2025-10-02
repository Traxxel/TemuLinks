using TemuLinks.DAL.Entities;
using TemuLinks.WebAPI.Services;

namespace TemuLinks.WebAPI.Middleware
{
    public class ApiKeyAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IApiKeyService apiKeyService)
        {
            // Skip authentication for public endpoints
            if (context.Request.Path.StartsWithSegments("/api/temulinks/public") ||
                context.Request.Path.StartsWithSegments("/swagger") ||
                context.Request.Path.StartsWithSegments("/health"))
            {
                await _next(context);
                return;
            }

            var apiKey = context.Request.Headers["X-API-Key"].FirstOrDefault();

            if (string.IsNullOrEmpty(apiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key is required");
                return;
            }

            var user = await apiKeyService.GetUserByApiKeyAsync(apiKey);
            if (user == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }

            // Add user to context for use in controllers
            context.Items["User"] = user;
            context.Items["UserId"] = user.Id;

            await _next(context);
        }
    }

    public static class ApiKeyAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyAuthenticationMiddleware>();
        }
    }
}
