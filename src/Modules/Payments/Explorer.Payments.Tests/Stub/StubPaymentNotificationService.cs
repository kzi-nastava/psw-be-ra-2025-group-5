using Explorer.Stakeholders.API.Dtos.Notifications;
using Explorer.Stakeholders.API.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Tests.Stub
{
    public class StubPaymentNotificationService : IPaymentNotificationService
    {
        public NotificationDto Create(NotificationDto dto)
        {
            return dto;
        }
        public void CreateGiftReceivedNotification(
         long recipientId,
         long tourId,
         string tourName,
         string donorName,
         string? message = null
        )
        {
           
        }
    }
}
