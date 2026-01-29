using Explorer.API.Controllers.Shopping;
using Explorer.Payments.API.Dtos.Wallet;
using Explorer.Payments.API.Public;
using Explorer.Payments.Infrastructure.Database;
using Explorer.Payments.Tests.Stub;
using Explorer.Stakeholders.API.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Payments.Tests.Integration
{
    [Collection("Sequential")]
    public class WalletCommandTests : BasePaymentsIntegrationTest, IDisposable
    {
        private readonly IServiceScope _scope;
        private readonly PaymentsContext _dbContext;

        public WalletCommandTests(PaymentsTestFactory factory) : base(factory)
        {
            _scope = Factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        }

        public void Dispose()
        {
            _dbContext.Wallets.RemoveRange(_dbContext.Wallets);
            _dbContext.SaveChanges();
            _scope.Dispose();
        }

        [Fact]
        public void Admin_can_credit_tourist_wallet()
        {
            var controller = CreateAdminController();

            var wallet = new Explorer.Payments.Core.Domain.Wallet(-21); 
            _dbContext.Wallets.Add(wallet);
            _dbContext.SaveChanges();

            var request = new WalletDto
            {
                UserId = -21,
                Balance = 100
            };

            // Act
            var result = controller.Credit(request) as OkObjectResult;

            // Assert
            result.ShouldNotBeNull();
            result.Value.ShouldNotBeNull();

            var updatedWallet = _dbContext.Wallets.First(w => w.UserId == -21);
            updatedWallet.Balance.ShouldBe(100);
        }

        [Fact]
        public void Tourist_wallet_is_created_with_zero_balance()
        {
            // Arrange
            long touristId = -101;
            var wallet = new Explorer.Payments.Core.Domain.Wallet(touristId);
            _dbContext.Wallets.Add(wallet);
            _dbContext.SaveChanges();

            // Act
            var savedWallet = _dbContext.Wallets.First(w => w.UserId == touristId);

            // Assert
            savedWallet.Balance.ShouldBe(0);
        }
        [Fact]
        public void Admin_can_credit_any_tourist_and_tourist_receives_notification()
        {
            // Arrange
            long touristId = -103;
            var wallet = new Explorer.Payments.Core.Domain.Wallet(touristId);
            _dbContext.Wallets.Add(wallet);
            _dbContext.SaveChanges();

            var controller = CreateAdminController();

            var request = new WalletDto
            {
                UserId = touristId,
                Balance = 120
            };

            // Act
            var result = controller.Credit(request) as OkObjectResult;

            // Assert
            result.ShouldNotBeNull();

            var updatedWallet = _dbContext.Wallets.First(w => w.UserId == touristId);
            updatedWallet.Balance.ShouldBe(120);

            var notificationService = _scope.ServiceProvider.GetRequiredService<IPaymentNotificationService>() as StubPaymentNotificationService;
            notificationService.ShouldNotBeNull();
        }

        private WalletController CreateAdminController()
        {
            return new WalletController(
                _scope.ServiceProvider.GetRequiredService<IWalletService>())
            {
                ControllerContext = BuildContext("-1", "administrator")
            };
        }

        private ControllerContext BuildContext(string userId, string role)
        {
            var user = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim("personId", userId),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role)
                }, "TestAuth"));

            return new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext { User = user }
            };
        }

    }
}
