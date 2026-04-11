using Hangfire.Dashboard;
using System.Text;

namespace MixxFit.API.Infrastructure.Hangfire
{
    public class HangfireAuthorizationFilter(string user, string pass) : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var auth = httpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (auth is null || !auth.StartsWith("Basic "))
            {
                Challenge(httpContext);
                return false;
            }

            var encodedCredentials = auth.Substring("Basic ".Length).Trim();
            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var parts = decodedCredentials.Split(':', 2);

            if (parts.Length != 2 || parts[0] != user || parts[1] != pass)
            {
                Challenge(httpContext);
                return false;
            }

            return true;
        }

        private void Challenge(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = 401;
            httpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Hangfire Dashboard\"";
        }
    }
}
