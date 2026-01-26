using Explorer.Stakeholders.API.Dtos.Notifications;
using Explorer.Stakeholders.API.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Tests.Stub
{
    public class TestPaymentNotificationService : IPaymentNotificationService
    {
        public List<NotificationDto> SentNotifications { get; } = new();

        public NotificationDto Create(NotificationDto notification)
        {
            SentNotifications.Add(notification);
            return notification;
        }

        public void CreateGiftReceivedNotification(
          long recipientId,
          long tourId,
          string tourName,
          string donorName,
          string? message = null
      )
        {
            SentNotifications.Add(new NotificationDto
            {
                UserId = recipientId,
                Title = "You received a gift!",
                Message = message ?? $"You received the tour '{tourName}' as a gift from {donorName}.",
                Type = "GiftReceived",
                TourId = tourId
            });
        }

    }
}
