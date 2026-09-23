using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MixxFit.API.Infrastructure.Persistence;

public class DatabaseSeeder(RoleManager<IdentityRole> roleManager, ILogger<DatabaseSeeder> logger)
{
    private static readonly string[] Roles = ["User", "Admin"];

    public async Task SeedRolesAsync()
    {
        foreach (var role in Roles)
        {
            if (await roleManager.RoleExistsAsync(role))
            {
                logger.LogInformation("Role '{Role}' already exists. Skipping seeding.", role);
                continue;
            }
            
            var result = await roleManager.CreateAsync(new IdentityRole(role));

            if (result.Succeeded)
            {
                logger.LogInformation("Seeded role '{Role}'.", role);
                continue;
            }

            logger.LogError(
                "Failed to seed role '{Role}': {Errors}",
                role,
                string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        logger.LogInformation("Role seeding completed.");
    }
}
