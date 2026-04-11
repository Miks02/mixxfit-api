using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Threading.RateLimiting;

namespace MixxFit.API.Infrastructure.RateLimiting
{
    public static class RateLimiterRegistration
    {
        public static void AddGlobalRateLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                    var hasMetadata = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter);

                    var detailMessage = hasMetadata
                        ? $"Request limit reached, try again after {retryAfter.TotalSeconds} seconds"
                        : "Request limit reached, please try again later.";

                    var problem = new ProblemDetails()
                    {
                        Title = "Too many requests",
                        Detail = detailMessage,
                        Status = options.RejectionStatusCode,
                        Instance = context.HttpContext.Request.Path
                    };

                    if (hasMetadata)
                    {
                        context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString();
                        problem.Extensions["RetryAfter"] = retryAfter.TotalSeconds;
                    }

                    await context.HttpContext.Response.WriteAsJsonAsync(problem, token);
                };

                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                {
                    var partitionKey = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    if (string.IsNullOrEmpty(partitionKey))
                        partitionKey = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ => new FixedWindowRateLimiterOptions()
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromSeconds(45)
                    });

                });

            });
        }
    }
}
