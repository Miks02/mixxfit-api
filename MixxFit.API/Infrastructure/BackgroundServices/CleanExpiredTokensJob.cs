using Microsoft.EntityFrameworkCore;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Infrastructure.BackgroundServices;

public class CleanExpiredTokensJob(
    ILogger<CleanExpiredTokensJob> logger,
    IServiceScopeFactory serviceScopeFactory,
    IConfiguration configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var retentionInDays = configuration.GetValue<int>("RefreshConfig:RetentionThresholdInDays");
        var cleaningInterval = configuration.GetValue<int>("RefreshConfig:CleanupIntervalInDays");
        
        logger.LogInformation("Cleaning service initialized.");;
        while(!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var dateDelta = DateTime.UtcNow.AddDays(-retentionInDays);
                using var scope = serviceScopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var affectedRows = await dbContext.RefreshTokens
                    .Where(rt => rt.ExpiresAt < dateDelta)
                    .ExecuteDeleteAsync(stoppingToken);

                if (affectedRows > 0)
                    logger.LogInformation("Deleted {AffectedRows} expired tokens.", affectedRows);

                await Task.Delay(TimeSpan.FromDays(cleaningInterval), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Service was canceled.");
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "A database error occurred while cleaning expired tokens.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while cleaning expired tokens.");
            }

        }
    }
}