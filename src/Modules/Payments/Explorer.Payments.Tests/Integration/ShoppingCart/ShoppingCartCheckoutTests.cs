using Explorer.API.Controllers.Shopping;
using Explorer.Payments.API.Public;
using Explorer.Payments.Infrastructure.Database;
using Explorer.Payments.Core.Domain;
using Explorer.Stakeholders.API.Internal;
using Explorer.Payments.Tests.Stub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.Payments.API.Dtos.ShoppingCart;

namespace Explorer.Payments.Tests.Integration.ShoppingCart;

[Collection("Sequential")]
public class ShoppingCartPaymentTests : BasePaymentsIntegrationTestWithNotifications
{
    public ShoppingCartPaymentTests(PaymentsTestFactoryWithNotifications factory) : base(factory) { }

    [Fact]
    public void Checkout_with_sufficient_balance_creates_payment_records()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var cartController = CreateController(scope);

        long touristId = -20;
        long tourId = -1;
        double tourPrice = 5.0;

        // Cleanup pre testa
        CleanupTestData(dbContext, touristId);

        var wallet = new Wallet(touristId);
        wallet.Credit(100);
        dbContext.Wallets.Add(wallet);
        dbContext.SaveChanges();

        cartController.AddOrderItem(touristId, tourId);

        var paymentCountBefore = dbContext.Payments.Count(p => p.TouristId == touristId);

        // Act
        var result = ((ObjectResult)cartController.Checkout(touristId).Result)?.Value as ShoppingCartDto;

        // Assert
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(0);
        result.Total.ShouldBe(0);

        var payments = dbContext.Payments.Where(p => p.TouristId == touristId).ToList();
        payments.Count.ShouldBe(paymentCountBefore + 1);

        var payment = payments.Last();
        payment.TouristId.ShouldBe(touristId);
        payment.TourId.ShouldBe(tourId);
        payment.Price.ShouldBe(2.5);
        payment.CreatedAt.ShouldNotBe(default(DateTime));
        payment.CreatedAt.ShouldBeLessThanOrEqualTo(DateTime.UtcNow);

        var walletAfter = dbContext.Wallets.First(w => w.UserId == touristId);
        walletAfter.Balance.ShouldBe(97.5);
    }

    [Fact]
    public void Checkout_with_insufficient_balance_fails()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var cartController = CreateController(scope);

        long touristId = -21;
        long tourId = -1;

        CleanupTestData(dbContext, touristId);

        var wallet = new Wallet(touristId);
        wallet.Credit(2);
        dbContext.Wallets.Add(wallet);
        dbContext.SaveChanges();

        cartController.AddOrderItem(touristId, tourId);

        var result = cartController.Checkout(touristId);

        result.Result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequest = result.Result as BadRequestObjectResult;
        badRequest.Value.ToString().ShouldContain("Not enough Adventure Coins");

        var payments = dbContext.Payments.Where(p => p.TouristId == touristId).ToList();
        payments.Count.ShouldBe(0);

        var walletAfter = dbContext.Wallets.First(w => w.UserId == touristId);
        walletAfter.Balance.ShouldBe(2);
    }

    [Fact]
    public void Checkout_creates_multiple_payment_records_for_multiple_tours()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var cartController = CreateController(scope);

        long touristId = -22;
        long tourId1 = -1;
        long tourId2 = -3;

        CleanupTestData(dbContext, touristId);

        var wallet = new Wallet(touristId);
        wallet.Credit(200);
        dbContext.Wallets.Add(wallet);
        dbContext.SaveChanges();

        cartController.AddOrderItem(touristId, tourId1);
        cartController.AddOrderItem(touristId, tourId2);

        // Act
        var result = ((ObjectResult)cartController.Checkout(touristId).Result)?.Value as ShoppingCartDto;

        // Assert
        var payments = dbContext.Payments.Where(p => p.TouristId == touristId).ToList();
        payments.Count.ShouldBeGreaterThan(0);

        foreach (var payment in payments)
        {
            payment.TouristId.ShouldBe(touristId);
            payment.Price.ShouldBeGreaterThanOrEqualTo(0);
            payment.CreatedAt.ShouldNotBe(default(DateTime));
        }
    }

    [Fact]
    public void Checkout_sends_notification_to_tourist()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var cartController = CreateController(scope);
        var notificationService = scope.ServiceProvider.GetRequiredService<IPaymentNotificationService>()
            as TestPaymentNotificationService;

        long touristId = -23;
        long tourId = -1;

        CleanupTestData(dbContext, touristId);

        var wallet = new Wallet(touristId);
        wallet.Credit(100);
        dbContext.Wallets.Add(wallet);
        dbContext.SaveChanges();

        cartController.AddOrderItem(touristId, tourId);

        notificationService.ShouldNotBeNull();
        var notificationCountBefore = notificationService.SentNotifications.Count;

        // Act
        cartController.Checkout(touristId);

        // Assert
        notificationService.SentNotifications.Count.ShouldBe(notificationCountBefore + 1);

        var notification = notificationService.SentNotifications.Last();
        notification.UserId.ShouldBe(touristId);
        notification.Type.ShouldBe("TourPurchased");
        notification.Title.ShouldBe("Tour purchased");
        notification.Message.ShouldContain("tour");
    }

    [Fact]
    public void Checkout_notification_message_singular_for_one_tour()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var cartController = CreateController(scope);
        var notificationService = scope.ServiceProvider.GetRequiredService<IPaymentNotificationService>()
            as TestPaymentNotificationService;

        long touristId = -24;

        CleanupTestData(dbContext, touristId);

        var wallet = new Wallet(touristId);
        wallet.Credit(100);
        dbContext.Wallets.Add(wallet);
        dbContext.SaveChanges();

        cartController.AddOrderItem(touristId, -1);

        // Act
        cartController.Checkout(touristId);

        // Assert
        notificationService.ShouldNotBeNull();
        var notification = notificationService.SentNotifications.Last();
        notification.Message.ShouldBe("New tour has been added to your collection.");
    }

    [Fact]
    public void Checkout_notification_message_plural_for_multiple_tours()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var cartController = CreateController(scope);
        var notificationService = scope.ServiceProvider.GetRequiredService<IPaymentNotificationService>()
            as TestPaymentNotificationService;

        long touristId = -25;

        CleanupTestData(dbContext, touristId);

        var wallet = new Wallet(touristId);
        wallet.Credit(200);
        dbContext.Wallets.Add(wallet);
        dbContext.SaveChanges();

        cartController.AddOrderItem(touristId, -1);
        cartController.AddOrderItem(touristId, -3);

        // Act
        var result = ((ObjectResult)cartController.Checkout(touristId).Result)?.Value as ShoppingCartDto;

        // Assert
        var payments = dbContext.Payments.Where(p => p.TouristId == touristId).ToList();

        notificationService.ShouldNotBeNull();
        var notification = notificationService.SentNotifications.Last();

        if (payments.Count > 1)
        {
            notification.Message.ShouldBe($"{payments.Count} new tours have been added to your collection.");
        }
        else
        {
            notification.Message.ShouldBe("New tour has been added to your collection.");
        }
    }

    [Fact]
    public void Checkout_only_charges_for_published_tours()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var cartController = CreateController(scope);

        long touristId = -26;

        CleanupTestData(dbContext, touristId);

        var wallet = new Wallet(touristId);
        wallet.Credit(200);
        dbContext.Wallets.Add(wallet);
        dbContext.SaveChanges();

        cartController.AddOrderItem(touristId, -1);
        cartController.AddOrderItem(touristId, -4);

        // Act
        var result = ((ObjectResult)cartController.Checkout(touristId).Result)?.Value as ShoppingCartDto;

        // Assert
        var walletAfter = dbContext.Wallets.First(w => w.UserId == touristId);
        walletAfter.Balance.ShouldBe(197.5);

        var payments = dbContext.Payments.Where(p => p.TouristId == touristId).ToList();
        payments.Count.ShouldBe(1);
        payments[0].TourId.ShouldBe(-1);
        payments[0].Price.ShouldBe(2.5);
    }

    [Fact]
    public void Payment_record_contains_all_required_fields()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var cartController = CreateController(scope);

        long touristId = -27;
        long tourId = -1;

        CleanupTestData(dbContext, touristId);

        var wallet = new Wallet(touristId);
        wallet.Credit(100);
        dbContext.Wallets.Add(wallet);
        dbContext.SaveChanges();

        cartController.AddOrderItem(touristId, tourId);

        var timeBeforeCheckout = DateTime.UtcNow;

        // Act
        cartController.Checkout(touristId);

        // Assert
        var payment = dbContext.Payments.First(p => p.TouristId == touristId && p.TourId == tourId);

        payment.Id.ShouldBeGreaterThan(0);
        payment.TouristId.ShouldBe(touristId);
        payment.TourId.ShouldBe(tourId);
        payment.Price.ShouldBe(2.5);
        payment.CreatedAt.ShouldNotBe(default(DateTime));
        payment.CreatedAt.ShouldBeGreaterThanOrEqualTo(timeBeforeCheckout);
        payment.CreatedAt.ShouldBeLessThanOrEqualTo(DateTime.UtcNow.AddSeconds(1));
    }

    [Fact]
    public void Checkout_with_existing_cart_from_test_data()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var cartController = CreateController(scope);

        long touristId = -5;


        var existingPayments = dbContext.Payments.Where(p => p.TouristId == touristId).ToList();
        dbContext.Payments.RemoveRange(existingPayments);

        var existingWallet = dbContext.Wallets.FirstOrDefault(w => w.UserId == touristId);
        if (existingWallet != null)
        {
            dbContext.Wallets.Remove(existingWallet);
        }

        cartController.AddOrderItem(touristId, -1);

        var wallet = new Wallet(touristId);
        wallet.Credit(100);
        dbContext.Wallets.Add(wallet);
        dbContext.SaveChanges();

        // Act
        var result = ((ObjectResult)cartController.Checkout(touristId).Result)?.Value as ShoppingCartDto;

        // Assert
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(0);

        var payments = dbContext.Payments.Where(p => p.TouristId == touristId).ToList();
        payments.Count.ShouldBe(1);
        payments[0].TourId.ShouldBe(-1);
        payments[0].Price.ShouldBe(2.5);

        var walletAfter = dbContext.Wallets.First(w => w.UserId == touristId);
        walletAfter.Balance.ShouldBe(97.5);
    }
    [Fact]
    public void Checkout_deducts_correct_total_for_multiple_tours()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var cartController = CreateController(scope);

        long touristId = -28;

        CleanupTestData(dbContext, touristId);

        var wallet = new Wallet(touristId);
        wallet.Credit(100);
        dbContext.Wallets.Add(wallet);
        dbContext.SaveChanges();

        cartController.AddOrderItem(touristId, -1);

        // Act
        cartController.Checkout(touristId);

        // Assert
        var walletAfter = dbContext.Wallets.First(w => w.UserId == touristId);
        var payments = dbContext.Payments.Where(p => p.TouristId == touristId).ToList();
        var totalPaid = payments.Sum(p => p.Price);

        walletAfter.Balance.ShouldBe(100 - totalPaid);
    }

    private static ShoppingCartController CreateController(IServiceScope scope)
    {
        return new ShoppingCartController(scope.ServiceProvider.GetRequiredService<IShoppingCartService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }

    private void CleanupTestData(PaymentsContext dbContext, long touristId)
    {
        var existingPayments = dbContext.Payments.Where(p => p.TouristId == touristId).ToList();
        dbContext.Payments.RemoveRange(existingPayments);

        var existingWallet = dbContext.Wallets.FirstOrDefault(w => w.UserId == touristId);
        if (existingWallet != null)
        {
            dbContext.Wallets.Remove(existingWallet);
        }

        var existingCart = dbContext.ShoppingCarts.FirstOrDefault(c => c.TouristId == touristId);
        if (existingCart != null)
        {
            dbContext.ShoppingCarts.Remove(existingCart);
        }

        dbContext.SaveChanges();
    }
}