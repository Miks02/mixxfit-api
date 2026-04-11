using Hangfire.Dashboard;

namespace MixxFit.API.Infrastructure.Hangfire
{
    public class HangfireAnonymousFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
