using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Comercios.Extensions
{
    public static class HeaderInjectionExtensions
    {
        public static IApplicationBuilder UseHeaderInjection(this IApplicationBuilder app)
        {
            var config = app.ApplicationServices.GetRequiredService<IConfiguration>();

            var expectedSecret = config["InternalSecret"];
            if (string.IsNullOrWhiteSpace(expectedSecret))
                expectedSecret = config["INTERNAL_SECRET"];
            if (string.IsNullOrWhiteSpace(expectedSecret))
                expectedSecret = config["expectedSecret"];
            if (string.IsNullOrWhiteSpace(expectedSecret))
                throw new InvalidOperationException("InternalSecret no configurado");

            return app.Use(async (context, next) =>
            {
                if (!context.Request.Headers.TryGetValue("X-Internal-Secret", out var receivedSecret))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { error = "Header de seguridad ausente" });
                    return;
                }

                if (!CryptographicOperations.FixedTimeEquals(
                        Encoding.UTF8.GetBytes(receivedSecret.ToString()),
                        Encoding.UTF8.GetBytes(expectedSecret)))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsJsonAsync(new { error = "Secreto inválido" });
                    return;
                }

                if (context.Request.Headers.TryGetValue("X-User-Id", out var userIdHeader) &&
                    !string.IsNullOrWhiteSpace(userIdHeader.ToString()))
                {
                    var userId = userIdHeader.ToString();
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId),
                    };

                    context.User = new ClaimsPrincipal(
                        new ClaimsIdentity(claims, "GatewayAuth")
                    );
                }

                await next();
            });
        }
    }
}
