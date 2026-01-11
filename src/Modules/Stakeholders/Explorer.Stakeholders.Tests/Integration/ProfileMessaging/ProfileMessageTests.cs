using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos.ProfileMessages;
using Explorer.Stakeholders.API.Public.ProfileMessages;
using Explorer.Stakeholders.Core.Domain.ProfileMessages;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.ProfileMessages;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.Core.Domain.Users;
using Explorer.Stakeholders.Core.UseCases.Administration.Social;
using Moq;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.ProfileMessaging
{
    [Collection("Sequential")]
    public class ProfileMessageTests : BaseStakeholdersIntegrationTest
    {
        public ProfileMessageTests(StakeholdersTestFactory factory) : base(factory) { }

        #region Unit Tests

        [Fact]
        public void Create_ValidMessage_ReturnsProfileMessageDto()
        {
            // Arrange
            var mockRepository = new Mock<IProfileMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();

            var createDto = new CreateMessageDto
            {
                Content = "Test message",
                AttachedResourceType = 0,
                AttachedResourceId = null
            };

            var profileMessage = new ProfileMessage(1, 2, "Test message", ProfileMessage.ResourceType.None, null);
            var mappedDto = new ProfileMessageDto
            {
                Id = 1,
                AuthorId = 1,
                ReceiverId = 2,
                Content = "Test message",
                AttachedResourceType = 0,
                AttachedResourceId = null,
                CreatedAt = DateTimeOffset.UtcNow,
                AuthorName = "TestUser"
            };

            var user = new User("testuser", "test@email.com", "password", UserRole.Tourist, true);

            mockRepository.Setup(r => r.Create(It.IsAny<ProfileMessage>())).Returns(profileMessage);
            mockMapper.Setup(m => m.Map<ProfileMessageDto>(It.IsAny<ProfileMessage>())).Returns(mappedDto);
            mockUserRepository.Setup(u => u.GetById(1)).Returns(user);

            var service = new ProfileMessageService(mockRepository.Object, mockUserRepository.Object, mockMapper.Object);

            // Act
            var result = service.Create(2, 1, createDto);

            // Assert
            result.ShouldNotBeNull();
            result.Content.ShouldBe("Test message");
            result.AuthorId.ShouldBe(1);
            result.ReceiverId.ShouldBe(2);
            result.AuthorName.ShouldBe("testuser");
            mockRepository.Verify(r => r.Create(It.IsAny<ProfileMessage>()), Times.Once);
            mockUserRepository.Verify(u => u.GetById(1), Times.Once);
        }

        [Fact]
        public void Create_WithTourResource_ReturnsProfileMessageDto()
        {
            // Arrange
            var mockRepository = new Mock<IProfileMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();

            var createDto = new CreateMessageDto
            {
                Content = "Check out this tour!",
                AttachedResourceType = 1, // Tour
                AttachedResourceId = 123
            };

            var profileMessage = new ProfileMessage(1, 2, "Check out this tour!", ProfileMessage.ResourceType.Tour, 123);
            var mappedDto = new ProfileMessageDto
            {
                Id = 1,
                AuthorId = 1,
                ReceiverId = 2,
                Content = "Check out this tour!",
                AttachedResourceType = 1,
                AttachedResourceId = 123,
                CreatedAt = DateTimeOffset.UtcNow,
                AuthorName = "TestUser"
            };

            var user = new User("testuser", "test@email.com", "password", UserRole.Tourist, true);

            mockRepository.Setup(r => r.Create(It.IsAny<ProfileMessage>())).Returns(profileMessage);
            mockMapper.Setup(m => m.Map<ProfileMessageDto>(It.IsAny<ProfileMessage>())).Returns(mappedDto);
            mockUserRepository.Setup(u => u.GetById(1)).Returns(user);

            var service = new ProfileMessageService(mockRepository.Object, mockUserRepository.Object, mockMapper.Object);

            // Act
            var result = service.Create(2, 1, createDto);

            // Assert
            result.ShouldNotBeNull();
            result.AttachedResourceType.ShouldBe(1);
            result.AttachedResourceId.ShouldBe(123);
            mockRepository.Verify(r => r.Create(It.IsAny<ProfileMessage>()), Times.Once);
        }

        [Fact]
        public void Create_UserNotFound_SetsEmptyAuthorName()
        {
        // Arrange
            var mockRepository = new Mock<IProfileMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();

            var createDto = new CreateMessageDto
            {
                Content = "Test message",
                AttachedResourceType = 0,
                AttachedResourceId = null
            };

            var profileMessage = new ProfileMessage(1, 2, "Test message", ProfileMessage.ResourceType.None, null);
            var mappedDto = new ProfileMessageDto
            {
                Id = 1,
                AuthorId = 1,
                ReceiverId = 2,
                Content = "Test message",
                AttachedResourceType = 0,
                AttachedResourceId = null,
                CreatedAt = DateTimeOffset.UtcNow,
                AuthorName = "TestUser"
            };

            mockRepository.Setup(r => r.Create(It.IsAny<ProfileMessage>())).Returns(profileMessage);
            mockMapper.Setup(m => m.Map<ProfileMessageDto>(It.IsAny<ProfileMessage>())).Returns(mappedDto);
            mockUserRepository.Setup(u => u.GetById(1)).Returns((User?)null);

            var service = new ProfileMessageService(mockRepository.Object, mockUserRepository.Object, mockMapper.Object);

            // Act
            var result = service.Create(2, 1, createDto);

            // Assert
            result.ShouldNotBeNull();
            result.AuthorName.ShouldBe(string.Empty);
        }

        [Fact]
        public void Update_ValidMessage_ReturnsUpdatedDto()
        {
        // Arrange
            var mockRepository = new Mock<IProfileMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();

            var updateDto = new UpdateMessageDto
            {
                Content = "Updated content",
                AttachedResourceType = 2,
                AttachedResourceId = 456
            };

            var existingMessage = new ProfileMessage(1, 2, "Original content", ProfileMessage.ResourceType.None, null);
            var updatedMessage = new ProfileMessage(1, 2, "Updated content", ProfileMessage.ResourceType.BlogPost, 456);
            var mappedDto = new ProfileMessageDto
            {
                Id = 1,
                AuthorId = 1,
                ReceiverId = 2,
                Content = "Updated content",
                AttachedResourceType = 2,
                AttachedResourceId = 456,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            mockRepository.Setup(r => r.GetById(1)).Returns(existingMessage);
            mockRepository.Setup(r => r.Update(It.IsAny<ProfileMessage>())).Returns(updatedMessage);
            mockMapper.Setup(m => m.Map<ProfileMessageDto>(It.IsAny<ProfileMessage>())).Returns(mappedDto);

            var service = new ProfileMessageService(mockRepository.Object, mockUserRepository.Object, mockMapper.Object);

            // Act
            var result = service.Update(1, 1, updateDto);

            // Assert
            result.ShouldNotBeNull();
            result.Content.ShouldBe("Updated content");
            result.AttachedResourceType.ShouldBe(2);
            result.AttachedResourceId.ShouldBe(456);
            mockRepository.Verify(r => r.GetById(1), Times.Once);
            mockRepository.Verify(r => r.Update(It.IsAny<ProfileMessage>()), Times.Once);
        }

        [Fact]
        public void Update_MessageNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var mockRepository = new Mock<IProfileMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();

            var updateDto = new UpdateMessageDto
            {
                Content = "Updated content",
                AttachedResourceType = 0,
                AttachedResourceId = null
            };

            mockRepository.Setup(r => r.GetById(1)).Returns((ProfileMessage?)null);

            var service = new ProfileMessageService(mockRepository.Object, mockUserRepository.Object, mockMapper.Object);

            // Act & Assert
            Should.Throw<NotFoundException>(() => service.Update(1, 1, updateDto))
            .Message.ShouldBe("Profile message with ID 1 not found.");
        }

        [Fact]
        public void Update_UnauthorizedUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var mockRepository = new Mock<IProfileMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();

            var updateDto = new UpdateMessageDto
            {
                Content = "Updated content",
                AttachedResourceType = 0,
                AttachedResourceId = null
            };

            var existingMessage = new ProfileMessage(1, 2, "Original content", ProfileMessage.ResourceType.None, null);
            mockRepository.Setup(r => r.GetById(1)).Returns(existingMessage);

            var service = new ProfileMessageService(mockRepository.Object, mockUserRepository.Object, mockMapper.Object);

            // Act & Assert
            Should.Throw<UnauthorizedAccessException>(() => service.Update(1, 2, updateDto))
            .Message.ShouldBe("Only the author can update this profile message.");
        }

        [Fact]
        public void Delete_ValidMessage_DeletesSuccessfully()
        {
            // Arrange
            var mockRepository = new Mock<IProfileMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();

            var existingMessage = new ProfileMessage(1, 2, "Test message", ProfileMessage.ResourceType.None, null);
            mockRepository.Setup(r => r.GetById(1)).Returns(existingMessage);

            var service = new ProfileMessageService(mockRepository.Object, mockUserRepository.Object, mockMapper.Object);

            // Act
            service.Delete(1, 1);

            // Assert
            mockRepository.Verify(r => r.GetById(1), Times.Once);
            mockRepository.Verify(r => r.Delete(1), Times.Once);
        }

        [Fact]
        public void Delete_MessageNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var mockRepository = new Mock<IProfileMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepository.Setup(r => r.GetById(1)).Returns((ProfileMessage?)null);

            var service = new ProfileMessageService(mockRepository.Object, mockUserRepository.Object, mockMapper.Object);

            // Act & Assert
            Should.Throw<NotFoundException>(() => service.Delete(1, 1))
            .Message.ShouldBe("Profile message with ID 1 not found.");
     }

        [Fact]
        public void Delete_UnauthorizedUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var mockRepository = new Mock<IProfileMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();

            var existingMessage = new ProfileMessage(1, 2, "Test message", ProfileMessage.ResourceType.None, null);
            mockRepository.Setup(r => r.GetById(1)).Returns(existingMessage);

            var service = new ProfileMessageService(mockRepository.Object, mockUserRepository.Object, mockMapper.Object);

            // Act & Assert
            Should.Throw<UnauthorizedAccessException>(() => service.Delete(1, 2))
            .Message.ShouldBe("Only the message author can delete this message.");
        }

        [Fact]
        public void GetById_ValidMessage_ReturnsProfileMessageDto()
        {
            // Arrange
            var mockRepository = new Mock<IProfileMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();

            var profileMessage = new ProfileMessage(1, 2, "Test message", ProfileMessage.ResourceType.None, null);
            var mappedDto = new ProfileMessageDto
            {
                Id = 1,
                AuthorId = 1,
                ReceiverId = 2,
                Content = "Test message",
                AttachedResourceType = 0,
                AttachedResourceId = null,
                CreatedAt = DateTimeOffset.UtcNow,
                AuthorName = "TestUser"
            };

            var user = new User("testuser", "test@email.com", "password", UserRole.Tourist, true);

            mockRepository.Setup(r => r.GetById(1)).Returns(profileMessage);
            mockMapper.Setup(m => m.Map<ProfileMessageDto>(It.IsAny<ProfileMessage>())).Returns(mappedDto);
            mockUserRepository.Setup(u => u.GetById(1)).Returns(user);

            var service = new ProfileMessageService(mockRepository.Object, mockUserRepository.Object, mockMapper.Object);

            // Act
            var result = service.GetById(1);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(1);
            result.Content.ShouldBe("Test message");
            result.AuthorName.ShouldBe("testuser");
            mockRepository.Verify(r => r.GetById(1), Times.Once);
            mockUserRepository.Verify(u => u.GetById(1), Times.Once);
        }

        [Fact]
        public void GetById_MessageNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var mockRepository = new Mock<IProfileMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepository.Setup(r => r.GetById(1)).Returns((ProfileMessage?)null);

            var service = new ProfileMessageService(mockRepository.Object, mockUserRepository.Object, mockMapper.Object);

            // Act & Assert
            Should.Throw<NotFoundException>(() => service.GetById(1))
            .Message.ShouldBe("Profile message with ID 1 not found.");
        }

        [Fact]
        public void GetById_UserNotFound_SetsEmptyAuthorName()
        {
            // Arrange
            var mockRepository = new Mock<IProfileMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();

            var profileMessage = new ProfileMessage(1, 2, "Test message", ProfileMessage.ResourceType.None, null);
            var mappedDto = new ProfileMessageDto
            {
                Id = 1,
                AuthorId = 1,
                ReceiverId = 2,
                Content = "Test message",
                AttachedResourceType = 0,
                AttachedResourceId = null,
                CreatedAt = DateTimeOffset.UtcNow,
                AuthorName = "TestUser"
            };

            mockRepository.Setup(r => r.GetById(1)).Returns(profileMessage);
            mockMapper.Setup(m => m.Map<ProfileMessageDto>(It.IsAny<ProfileMessage>())).Returns(mappedDto);
            mockUserRepository.Setup(u => u.GetById(1)).Returns((User?)null);

            var service = new ProfileMessageService(mockRepository.Object, mockUserRepository.Object, mockMapper.Object);

            // Act
            var result = service.GetById(1);

            // Assert
            result.ShouldNotBeNull();
            result.AuthorName.ShouldBe(string.Empty);
        }

        [Fact]
        public void GetByReceiverId_ValidReceiver_ReturnsMessageList()
        {
            // Arrange
      var mockRepository = new Mock<IProfileMessageRepository>();
var mockUserRepository = new Mock<IUserRepository>();
          var mockMapper = new Mock<IMapper>();

          var messages = new List<ProfileMessage>
      {
    new ProfileMessage(1, 2, "Message 1", ProfileMessage.ResourceType.None, null),
                new ProfileMessage(3, 2, "Message 2", ProfileMessage.ResourceType.Tour, 123)
            };

var mappedDtos = new List<ProfileMessageDto>
       {
     new ProfileMessageDto { Id = 1, AuthorId = 1, ReceiverId = 2, Content = "Message 1", AuthorName = "User1" },
       new ProfileMessageDto { Id = 2, AuthorId = 3, ReceiverId = 2, Content = "Message 2", AuthorName = "User3" }
       };

    var user1 = new User("user1", "user1@email.com", "password", UserRole.Tourist, true);
        var user3 = new User("user3", "user3@email.com", "password", UserRole.Tourist, true);

  mockRepository.Setup(r => r.GetByReceiverId(1, 2)).Returns(messages);
            mockMapper.Setup(m => m.Map<List<ProfileMessageDto>>(It.IsAny<List<ProfileMessage>>())).Returns(mappedDtos);
        mockUserRepository.Setup(u => u.GetById(1)).Returns(user1);
    mockUserRepository.Setup(u => u.GetById(3)).Returns(user3);

   var service = new ProfileMessageService(mockRepository.Object, mockUserRepository.Object, mockMapper.Object);

          // Act
            var result = service.GetByReceiverId(1, 2);

         // Assert
    result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result[0].AuthorName.ShouldBe("user1");
          result[1].AuthorName.ShouldBe("user3");
            mockRepository.Verify(r => r.GetByReceiverId(1, 2), Times.Once);
          mockUserRepository.Verify(u => u.GetById(1), Times.Once);
    mockUserRepository.Verify(u => u.GetById(3), Times.Once);
        }

        [Fact]
        public void GetByReceiverId_NoMessages_ReturnsEmptyList()
  {
            // Arrange
      var mockRepository = new Mock<IProfileMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
     var mockMapper = new Mock<IMapper>();

            var messages = new List<ProfileMessage>();
     var mappedDtos = new List<ProfileMessageDto>();

 mockRepository.Setup(r => r.GetByReceiverId(1, 2)).Returns(messages);
      mockMapper.Setup(m => m.Map<List<ProfileMessageDto>>(It.IsAny<List<ProfileMessage>>())).Returns(mappedDtos);

     var service = new ProfileMessageService(mockRepository.Object, mockUserRepository.Object, mockMapper.Object);

         // Act
      var result = service.GetByReceiverId(1, 2);

            // Assert
        result.ShouldNotBeNull();
result.ShouldBeEmpty();
       mockRepository.Verify(r => r.GetByReceiverId(1, 2), Times.Once);
        }

     [Fact]
        public void GetByReceiverId_SomeUsersNotFound_SetsEmptyAuthorNames()
{
       // Arrange
      var mockRepository = new Mock<IProfileMessageRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
     var mockMapper = new Mock<IMapper>();

            var messages = new List<ProfileMessage>
     {
 new ProfileMessage(1, 2, "Message 1", ProfileMessage.ResourceType.None, null),
            new ProfileMessage(999, 2, "Message 2", ProfileMessage.ResourceType.None, null)
         };

         var mappedDtos = new List<ProfileMessageDto>
   {
      new ProfileMessageDto { Id = 1, AuthorId = 1, ReceiverId = 2, Content = "Message 1", AuthorName = "User1" },
         new ProfileMessageDto { Id = 2, AuthorId = 999, ReceiverId = 2, Content = "Message 2", AuthorName = "User999" }
 };

            var user1 = new User("user1", "user1@email.com", "password", UserRole.Tourist, true);

            mockRepository.Setup(r => r.GetByReceiverId(1, 2)).Returns(messages);
          mockMapper.Setup(m => m.Map<List<ProfileMessageDto>>(It.IsAny<List<ProfileMessage>>())).Returns(mappedDtos);
            mockUserRepository.Setup(u => u.GetById(1)).Returns(user1);
      mockUserRepository.Setup(u => u.GetById(999)).Returns((User?)null);

            var service = new ProfileMessageService(mockRepository.Object, mockUserRepository.Object, mockMapper.Object);

        // Act
  var result = service.GetByReceiverId(1, 2);

            // Assert
          result.ShouldNotBeNull();
result.Count.ShouldBe(2);
            result[0].AuthorName.ShouldBe("user1");
         result[1].AuthorName.ShouldBe(string.Empty);
      }

   #endregion
    }
}
