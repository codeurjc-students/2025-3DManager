using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Notifications;

namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface INotificationManager
    {
        bool CreateNotification(int userId, NotificationType type, string message, string? email, out BaseError? error);
        List<NotificationObject> GetUnreadNotifications(int userId, out BaseError? error);
        bool NotificationMarkAsRead(int notificationId, out BaseError? error);
    }

}
