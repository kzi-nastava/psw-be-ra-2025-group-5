using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos.ClubMessages;
using Explorer.Stakeholders.API.Public.ClubMessages;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq;

namespace Explorer.Stakeholders.Tests.Integration.ClubMessaging
{
    [Collection("Sequential")]
    public class ClubMessageTests : BaseStakeholdersIntegrationTest
    {
        public ClubMessageTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Successfully_creates_message_with_tour_resource()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubMessageService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var createDto = new CreateClubMessageDto
            {
                Content = "Nova tura preporuka",
                AttachedResourceType = 1,
                AttachedResourceId = 10
            };

            // Act
            var result = service.Create(-41, -23, createDto);

            // Assert
            result.ShouldNotBeNull();
            result.AttachedResourceType.ShouldBe(1);
            result.AttachedResourceId.ShouldBe(10);

            dbContext.ChangeTracker.Clear();
            var storedMessage = dbContext.ClubMessages.FirstOrDefault(m => m.Id == result.Id);
            storedMessage.ShouldNotBeNull();
        }

        [Fact]
        public void Fails_to_create_message_when_not_member()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubMessageService>();

            var createDto = new CreateClubMessageDto
            {
                Content = "Pokušaj slanja poruke",
                AttachedResourceType = 0,
                AttachedResourceId = null
            };

            // Act & Assert - User -99 is not a member of club -41
            Should.Throw<UnauthorizedAccessException>(() =>
                service.Create(-41, -99, createDto)
            );
        }

        [Fact]
        public void Fails_to_create_message_exceeding_280_characters()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubMessageService>();

            var createDto = new CreateClubMessageDto
            {
                Content = new string('a', 281),
                AttachedResourceType = 0,
                AttachedResourceId = null
            };

            // Act & Assert
            Should.Throw<ArgumentException>(() =>
                service.Create(-41, -23, createDto)
            );
        }

        [Fact]
        public void Successfully_updates_message_by_author()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubMessageService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var updateDto = new UpdateClubMessageDto
            {
                Content = "Izmenjena poruka",
                AttachedResourceType = 0,
                AttachedResourceId = null
            };

            // Act - Update message -1 created by user -21
            var result = service.Update(-1, -21, updateDto);

            // Assert
            result.ShouldNotBeNull();
            result.Content.ShouldBe("Izmenjena poruka");
            result.UpdatedAt.ShouldNotBeNull();
        }

        [Fact]
        public void Fails_to_update_message_by_non_author()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubMessageService>();

            var updateDto = new UpdateClubMessageDto
            {
                Content = "Pokušaj izmene",
                AttachedResourceType = 0,
                AttachedResourceId = null
            };

            // Act & Assert - User -23 trying to update message -1 created by user -21
            Should.Throw<UnauthorizedAccessException>(() =>
                service.Update(-1, -23, updateDto)
            );
        }

        [Fact]
        public void Club_owner_can_delete_any_message()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubMessageService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            // Act - User -21 is club owner, deletes message -2 created by -23
            service.Delete(-2, -21, true);

            // Assert
            dbContext.ChangeTracker.Clear();
            var deletedMessage = dbContext.ClubMessages.Find(-2L);
            deletedMessage.ShouldBeNull();
        }

        [Fact]
        public void Successfully_gets_all_messages_for_club()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubMessageService>();

            // Act
            var result = service.GetByClubId(-41);

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThanOrEqualTo(2);
            result.All(m => m.ClubId == -41).ShouldBeTrue();
        }
    }
}

