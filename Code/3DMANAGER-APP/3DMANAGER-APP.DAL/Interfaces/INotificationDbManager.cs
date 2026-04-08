using _3DMANAGER_APP.DAL.Models.Notifications;

namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface INotificationDbManager
    {
        List<NotificationDbObject> GetUnreadNotifications(int userId, out bool error);
        bool InsertNotification(NotificationDbObject notification, out int newId);
        bool NotificationMarkAsRead(int notificationId);
    }

}
