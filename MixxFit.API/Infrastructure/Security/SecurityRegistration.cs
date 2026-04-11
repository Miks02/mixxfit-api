using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Infrastructure.Security
{
    public static class SecurityRegistration
    {
        public static void AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JwtConfig:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["JwtConfig:Audience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtConfig:Token"]!))
                    };
                });

            services.AddAuthorization();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICookieProvider, CookieProvider>();
            services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        }
    }
}   