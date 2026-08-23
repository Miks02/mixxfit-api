using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MixxFit.API.Infrastructure.Exceptions
{
    public class TokensRevokedExceptionHandler(
        ILogger<TokensRevokedExceptionHandler> logger,
        IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not AllTokensRevokedException)
                return false;

            var userIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
            var traceId = httpContext.TraceIdentifier;

            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

            logger.LogWarning("Security breach detected! All user's sessions have been revoked. User IP: {UserIp} . TraceID: {TraceId}", userIp, traceId);
            
            httpContext.Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/"
            });
            
            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Type = "urn:mixxfit-api:error:AuthError.SecurityBreach",
                    Title = "Unauthorized",
                    Detail = "All client sessions have been revoked due to a possible security breach."
                }
            });

        }
    }
}
