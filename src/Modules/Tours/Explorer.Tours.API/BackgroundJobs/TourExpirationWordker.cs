using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Explorer.Tours.API.Public;

public class TourExpirationWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public TourExpirationWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ITourExecutionService>();
                service.ExpireOldTours();
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
