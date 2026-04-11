using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using MixxFit.API.Features.Exercises.CleanUpExercisesJob;

namespace MixxFit.API.Infrastructure.Hangfire
{
    public static class HangfireExtensions
    {

        public static void AddHangfireSetup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire((sp, config) =>
            {
                config.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(configuration.GetConnectionString("PostgresConnection")));
            });
            services.AddHangfireServer();
        }

        public static void AddRecurringJobs(this IServiceCollection services)
            => services.AddScoped<CleanUpExercisesJob>();
     
        public static void UseHangfireDashboardWithAuthorization(this WebApplication app)
        {
            var config = app.Configuration;
            var user = config["Hangfire:Username"] ?? "";
            var password = config["Hangfire:Password"] ?? "";

            if (app.Environment.IsProduction() && (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password)))
                throw new InvalidOperationException("Hangfire credentials are not set");

            IDashboardAuthorizationFilter authFilter = app.Environment.IsDevelopment() 
                ? new HangfireAnonymousFilter()
                : new HangfireAuthorizationFilter(user, password);

            var options = new DashboardOptions
            {
                Authorization = [authFilter]
            };
            app.UseHangfireDashboard("/hangfire", options);
        }

        public static void UseRecurringJobs(this WebApplication app)
        {
            RecurringJob.AddOrUpdate<CleanUpExercisesJob>(
                "cleanup-exercises",
                job => job.Execute(),
                Cron.Daily(0));

        }
    }
}
