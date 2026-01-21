using Explorer.Stakeholders.API.Public.Users;
using Explorer.Stakeholders.Core.Domain.Users;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace Explorer.Stakeholders.Tests.Integration.Premium;

[Collection("Sequential")]
public class UserPremiumCommandTests : BaseStakeholdersIntegrationTest
{
    public UserPremiumCommandTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Is_premium_returns_true_for_active_premium()
    {
        using var scope = Factory.Services.CreateScope();
        var premiumService = scope.ServiceProvider.GetRequiredService<IPremiumService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var userId = -1L;
        var validUntil = DateTime.UtcNow.AddDays(10);

        // Act
        premiumService.GrantPremium(userId, validUntil);
        var isPremium = premiumService.IsPremium(userId);

        // Assert
        isPremium.ShouldBeTrue();

        var stored = dbContext.UserPremiums.Single(p => p.UserId == userId);
        stored.ValidUntil.ShouldNotBeNull();
        stored.ValidUntil.Value.ShouldBeGreaterThan(DateTime.UtcNow);
    }

    [Fact]
    public void Is_premium_returns_false_when_no_premium()
    {
        using var scope = Factory.Services.CreateScope();
        var premiumService = scope.ServiceProvider.GetRequiredService<IPremiumService>();

        var userId = -999L;

        // Act
        var isPremium = premiumService.IsPremium(userId);

        // Assert
        isPremium.ShouldBeFalse();
    }

    [Fact]
    public void Removes_premium_successfully()
    {
        using var scope = Factory.Services.CreateScope();
        var premiumService = scope.ServiceProvider.GetRequiredService<IPremiumService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var userId = -2L;

        // Arrange
        premiumService.GrantPremium(userId, DateTime.UtcNow.AddDays(30));

        // Act
        premiumService.RemovePremium(userId);

        // Assert
        premiumService.IsPremium(userId).ShouldBeFalse();
        dbContext.UserPremiums.FirstOrDefault(p => p.UserId == userId).ShouldBeNull();
    }

    [Fact]
    public void Expired_premium_is_not_active()
    {
        using var scope = Factory.Services.CreateScope();
        var premiumService = scope.ServiceProvider.GetRequiredService<IPremiumService>();

        var userId = -3L;

        // Arrange
        premiumService.GrantPremium(userId, DateTime.UtcNow.AddDays(-1));

        // Act
        var isPremium = premiumService.IsPremium(userId);

        // Assert
        isPremium.ShouldBeFalse();
    }
}
