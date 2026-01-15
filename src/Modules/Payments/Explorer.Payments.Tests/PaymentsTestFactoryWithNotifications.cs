using Explorer.BuildingBlocks.Tests;
using Explorer.Payments.Infrastructure.Database;
using Explorer.Payments.Tests.Stub;
using Explorer.Stakeholders.API.Internal;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Explorer.Payments.Tests;

public class PaymentsTestFactoryWithNotifications : BaseTestFactory<PaymentsContext>
{
    protected override IServiceCollection ReplaceNeededDbContexts(IServiceCollection services)
    {
        // Replace PaymentsContext
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<PaymentsContext>));
        services.Remove(descriptor!);
        services.AddDbContext<PaymentsContext>(SetupTestContext());

        // Replace ToursContext
        descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ToursContext>));
        services.Remove(descriptor!);
        services.AddDbContext<ToursContext>(SetupTestContext());

        // Replace notification service sa TestPaymentNotificationService
        var notificationDescriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(IPaymentNotificationService));
        if (notificationDescriptor != null)
        {
            services.Remove(notificationDescriptor);
        }

        // Registruj kao Singleton da bi mogao da se deli između testova
        services.AddScoped<IPaymentNotificationService, TestPaymentNotificationService>();

        return services;
    }
}