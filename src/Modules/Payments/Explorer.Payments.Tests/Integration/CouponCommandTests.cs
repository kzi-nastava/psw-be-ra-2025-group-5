using Explorer.API.Controllers.Shopping;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Explorer.Payments.Tests.Integration;

[Collection("Sequential")]
public class CouponCommandTests : BasePaymentsIntegrationTest
{
    public CouponCommandTests(PaymentsTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

        var dto = new CouponDto
        {
            Code = "NEWCOD10",
            Percentage = 10,
            AuthorId = -1,
            TourId = -1,
            ExpirationDate = DateTime.UtcNow.AddDays(30)
        };

        var result = ((ObjectResult)controller.Create(dto).Result)?.Value as CouponDto;

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Code.ShouldBe(dto.Code);

        var stored = dbContext.Coupons.Single(c => c.Id == result.Id);
        stored.Code.ShouldBe(dto.Code);
    }

    [Fact]
    public void Create_fails_invalid_percentage()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var dto = new CouponDto
        {
            Code = "BADPRC01",
            Percentage = 150,
            AuthorId = -1
        };

        Should.Throw<Exception>(() => controller.Create(dto));
    }

    [Fact]
    public void Create_fails_duplicate_code()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var dto = new CouponDto
        {
            Code = "DUPL0001",
            Percentage = 10,
            AuthorId = -1
        };

        controller.Create(dto);

        Should.Throw<Exception>(() => controller.Create(dto));
    }

    [Fact]
    public void Updates()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

        // Create
        var created = ((ObjectResult)controller.Create(new CouponDto
        {
            Code = "UPD00001",
            Percentage = 10,
            AuthorId = -1
        }).Result)?.Value as CouponDto;

        created.ShouldNotBeNull();

        var tracked = dbContext.Coupons.Local.FirstOrDefault(c => c.Id == created.Id);
        if (tracked != null) dbContext.Entry(tracked).State = EntityState.Detached;

        // Update
        created.Percentage = 25;
        var result = ((ObjectResult)controller.Update(created).Result)?.Value as CouponDto;

        result.ShouldNotBeNull();
        result.Percentage.ShouldBe(25);

        // Assert database
        dbContext.Coupons.Single(c => c.Id == created.Id).Percentage.ShouldBe(25);
    }

    [Fact]
    public void Update_fails_non_existing_id()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var dto = new CouponDto
        {
            Id = -999,
            Code = "NOEXIST1",
            Percentage = 10,
            AuthorId = -1
        };

        Should.Throw<Exception>(() => controller.Update(dto));
    }

    [Fact]
    public void Update_fails_invalid_percentage()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var created = ((ObjectResult)controller.Create(new CouponDto
        {
            Code = "BADUPD01",
            Percentage = 10,
            AuthorId = -1
        }).Result)?.Value as CouponDto;

        created.ShouldNotBeNull();
        created.Percentage = 200;

        Should.Throw<Exception>(() => controller.Update(created));
    }

    [Fact]
    public void Deletes()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

        var created = ((ObjectResult)controller.Create(new CouponDto
        {
            Code = "DEL00001",
            Percentage = 15,
            AuthorId = -2
        }).Result)?.Value as CouponDto;

        var response = controller.Delete(created.Id);

        response.ShouldBeOfType<NoContentResult>();
        dbContext.Coupons.FirstOrDefault(c => c.Id == created.Id).ShouldBeNull();
    }

    [Fact]
    public void Gets_by_id()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var created = ((ObjectResult)controller.Create(new CouponDto
        {
            Code = "GETID001",
            Percentage = 20,
            AuthorId = -1
        }).Result)?.Value as CouponDto;

        var result = ((ObjectResult)controller.Get(created.Id).Result)?.Value as CouponDto;

        result.ShouldNotBeNull();
        result.Id.ShouldBe(created.Id);
    }

    [Fact]
    public void Gets_by_author()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetByAuthor(-5).Result)?.Value as List<CouponDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBeGreaterThanOrEqualTo(2);
        result.All(c => c.AuthorId == -5).ShouldBeTrue();
    }


    [Fact]
    public void Gets_by_code()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        controller.Create(new CouponDto
        {
            Code = "CODE1234",
            Percentage = 20,
            AuthorId = -3
        });

        var result = ((ObjectResult)controller.GetByCode("CODE1234").Result)?.Value as CouponDto;

        result.ShouldNotBeNull();
        result.Code.ShouldBe("CODE1234");
    }

    private static CouponController CreateController(IServiceScope scope)
    {
        return new CouponController(scope.ServiceProvider.GetRequiredService<ICouponService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
