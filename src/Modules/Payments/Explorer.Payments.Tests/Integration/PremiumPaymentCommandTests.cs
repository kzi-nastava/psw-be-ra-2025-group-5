using Explorer.API.Controllers.Users;
using Explorer.Payments.API.Public;
using Explorer.Payments.Infrastructure.Database;
using Explorer.Stakeholders.API.Public.Users;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq;

namespace Explorer.Payments.Tests.Integration;

[Collection("Sequential")]
public class PremiumPaymentCommandTests : BasePaymentsIntegrationTest
{
    public PremiumPaymentCommandTests(PaymentsTestFactory factory) : base(factory) { }

    [Fact]
    public void Purchase_premium_creates_premium_and_charges_wallet()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var paymentsContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var stakeholdersContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var walletBefore = paymentsContext.Wallets.Single(w => w.TouristId == -1).Balance;

        // Act
        var response = controller.Purchase();

        // Assert controller
        response.ShouldBeOfType<NoContentResult>();

        // Assert premium created
        var premium = stakeholdersContext.UserPremiums.SingleOrDefault(p => p.UserId == -1);
        premium.ShouldNotBeNull();
        premium.ValidUntil.Value.ShouldBeGreaterThan(DateTime.UtcNow);

        // Assert wallet charged
        var walletAfter = paymentsContext.Wallets.Single(w => w.TouristId == -1).Balance;
        walletAfter.ShouldBe(walletBefore - 50);
    }

    

    [Fact]
    public void Extend_premium_extends_valid_until_and_charges_wallet()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var paymentsContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var stakeholdersContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        // Arrange – kupi premium
        controller.Purchase();
        var premiumBefore = stakeholdersContext.UserPremiums.Single(p => p.UserId == -1);
        var validUntilBefore = premiumBefore.ValidUntil;
        var walletBefore = paymentsContext.Wallets.Single(w => w.TouristId == -1).Balance;

        // Act – produži
        var response = controller.Extend();

        // Assert controller
        response.ShouldBeOfType<NoContentResult>();

        // Assert premium extended
        var premiumAfter = stakeholdersContext.UserPremiums.Single(p => p.UserId == -1);
        premiumAfter.ValidUntil.Value.ShouldBeGreaterThan(validUntilBefore.Value);

        // Assert wallet charged
        var walletAfter = paymentsContext.Wallets.Single(w => w.TouristId == -1).Balance;
        walletAfter.ShouldBe(walletBefore - 50);
    }

    private static PremiumController CreateController(IServiceScope scope)
    {
        return new PremiumController(
            scope.ServiceProvider.GetRequiredService<IPremiumPaymentService>(),
            scope.ServiceProvider.GetRequiredService<IPremiumService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
