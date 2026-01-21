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

    private void CleanupPremium(long userId, StakeholdersContext dbContext)
    {
        var existing = dbContext.UserPremiums.Where(p => p.UserId == userId).ToList();
        if (existing.Any())
        {
            dbContext.UserPremiums.RemoveRange(existing);
            dbContext.SaveChanges();
        }
    }

    [Fact]
    public void Is_premium_returns_true_for_active_premium()
    {
        using var scope = Factory.Services.CreateScope();
        var premiumService = scope.ServiceProvider.GetRequiredService<IPremiumService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var userId = -21L; // turista1
        CleanupPremium(userId, dbContext);

        var validUntil = DateTime.UtcNow.AddDays(10);
        premiumService.GrantPremium(userId, validUntil);

        premiumService.IsPremium(userId).ShouldBeTrue();

        var stored = dbContext.UserPremiums.Single(p => p.UserId == userId);
        stored.ValidUntil.Value.ShouldBeGreaterThan(DateTime.UtcNow);
    }

    [Fact]
    public void Is_premium_returns_false_when_no_premium()
    {
        using var scope = Factory.Services.CreateScope();
        var premiumService = scope.ServiceProvider.GetRequiredService<IPremiumService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var userId = -24L; // turista4
        CleanupPremium(userId, dbContext);

        premiumService.IsPremium(userId).ShouldBeFalse();
    }

    [Fact]
    public void Removes_premium_successfully()
    {
        using var scope = Factory.Services.CreateScope();
        var premiumService = scope.ServiceProvider.GetRequiredService<IPremiumService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var userId = -23L; // turista3
        CleanupPremium(userId, dbContext);

        premiumService.GrantPremium(userId, DateTime.UtcNow.AddDays(30));
        premiumService.RemovePremium(userId);

        premiumService.IsPremium(userId).ShouldBeFalse();
        dbContext.UserPremiums.FirstOrDefault(p => p.UserId == userId).ShouldBeNull();
    }

    [Fact]
    public void Expired_premium_is_not_active()
    {
        using var scope = Factory.Services.CreateScope();
        var premiumService = scope.ServiceProvider.GetRequiredService<IPremiumService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var userId = -21L; // turista1
        CleanupPremium(userId, dbContext);

        // Kreiraj premium koji je već istekao
        premiumService.GrantPremium(userId, DateTime.UtcNow.AddDays(-51));

        premiumService.IsPremium(userId).ShouldBeFalse();

    }
}
