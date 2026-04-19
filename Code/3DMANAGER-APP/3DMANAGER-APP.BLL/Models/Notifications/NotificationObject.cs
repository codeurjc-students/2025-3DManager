namespace _3DMANAGER_APP.BLL.Models.Notifications
{
    public class NotificationObject
    {
        public int NotificationId { get; set; }
        public bool NotificationIsRead { get; set; }
        public string NotificationMessage { get; set; } = string.Empty;
        public DateTime NotificationRegisterDate { get; set; }
        public int NotificationUserId { get; set; }
        public int NotificationType { get; set; }
    }

}
