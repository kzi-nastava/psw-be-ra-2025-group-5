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
    }
}
