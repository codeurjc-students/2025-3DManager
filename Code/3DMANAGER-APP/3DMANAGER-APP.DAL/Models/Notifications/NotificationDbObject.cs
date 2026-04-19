using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Notifications
{
    public class NotificationDbObject
    {
        public int NotificationId { get; set; }
        private const string NotificationIdColumnName = "NOTIFICATION_ID";
        public bool NotificationIsRead { get; set; }
        private const string NotificationIsReadColumnName = "NOTIFICATION_IS_READ";
        public string NotificationMessage { get; set; } = string.Empty;
        private const string NotificationMessageColumnName = "NOTIFICATION_MESSAGE";
        public DateTime NotificationRegisterDate { get; set; }
        private const string NotificationRegisterDateColumnName = "NOTIFICATION_DATE";
        public int NotificationUserId { get; set; }
        private const string NotificationUserIdColumnName = "NOTIFICATION_USER_ID";
        public int NotificationType { get; set; }
        private const string NotificationTypeColumnName = "NOTIFICATION_TYPE";

        public NotificationDbObject()
        {
        }

        public NotificationDbObject Create(DataRow row)
        {
            var obj = new NotificationDbObject();

            obj.NotificationId = row.Field<int>(NotificationIdColumnName);
            obj.NotificationIsRead = row.Field<bool>(NotificationIsReadColumnName);
            obj.NotificationMessage = row.Field<string>(NotificationMessageColumnName);
            obj.NotificationRegisterDate = row.Field<DateTime>(NotificationRegisterDateColumnName);
            obj.NotificationUserId = row.Field<int>(NotificationUserIdColumnName);
            obj.NotificationType = row.Field<int>(NotificationTypeColumnName);

            return obj;
        }
    }
}
