using Explorer.Stakeholders.API.Public.Badges;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Explorer.API.BackgroundJobs;

public class DailyBadgeCheckBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DailyBadgeCheckBackgroundService> _logger;

    public DailyBadgeCheckBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<DailyBadgeCheckBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Daily Badge Check Background Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var next2AM = DateTime.UtcNow.Date.AddDays(1).AddHours(2);
            
            if (now.Hour >= 2)
            {
                next2AM = DateTime.UtcNow.Date.AddDays(1).AddHours(2);
            }
            else
            {
                next2AM = DateTime.UtcNow.Date.AddHours(2);
            }

            var delay = next2AM - now;
            
            _logger.LogInformation("Next Veteran badge check scheduled at {NextRun} (in {Delay})", next2AM, delay);

            await Task.Delay(delay, stoppingToken);

            if (!stoppingToken.IsCancellationRequested)
            {
                await RunDailyCheck();
            }
        }
    }

    private async Task RunDailyCheck()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var badgeCheckService = scope.ServiceProvider.GetRequiredService<IDailyBadgeCheckService>();
            
            await Task.Run(() => badgeCheckService.CheckAndAwardVeteranBadges());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running daily badge check");
        }
    }
}
