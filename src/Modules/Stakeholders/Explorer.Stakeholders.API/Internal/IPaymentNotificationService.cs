using Explorer.Stakeholders.API.Dtos.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Internal
{
    public interface IPaymentNotificationService
    {
        NotificationDto Create(NotificationDto notification);
        void CreateGiftReceivedNotification(long recipientId, long tourId, string tourName, string donorName, string message);
    }
}
