using Microsoft.EntityFrameworkCore;

namespace MixxFit.API.Infrastructure.Persistence
{
    public static class PersistenceRegistration
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PostgresConnectionLocal"));
            });
        }   
    }
}
