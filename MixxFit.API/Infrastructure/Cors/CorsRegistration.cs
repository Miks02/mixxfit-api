namespace MixxFit.API.Infrastructure.Cors
{
    public static class CorsRegistration
    {
        public static void AddCorsPolicies(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowCors", policyBuilder =>
                {
                    policyBuilder
                        .WithOrigins("https://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });

                options.AddPolicy("ProdCors", policyBuilder =>
                {
                    policyBuilder
                        .WithOrigins("https://vitalops-web.onrender.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });

            });
        }
    }
}
