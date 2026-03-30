using _3DMANAGER_APP.DAL.Models.File;
using System.Data;

namespace _3DMANAGER_APP.DAL.Models.User
{
    public class UserDetailDbObject
    {
        public int UserId { get; set; }

        private const string UserIdColumnName = "3DMANAGER_USER_ID";
        public string UserName { get; set; }
        private const string UserNameColumnName = "3DMANAGER_USER_NAME";
        public string UserRole { get; set; }
        private const string UserRoleColumnName = "USER_ROLE";
        public string UserEmail { get; set; }
        private const string UserEmailColumnName = "3DMANAGER_USER_EMAIL";
        public DateTime UserCreateDate { get; set; }
        private const string UserCreateDateColumnName = "USER_CREATE_DATE";
        public int UserTotalPrints { get; set; }
        private const string UserTotalPrintsColumnName = "USER_TOTAL_PRINTS";
        public double UserTotalHours { get; set; }
        private const string UserTotalHoursColumnName = "USER_TOTAL_HOURS";
        public int UserPrintedPrints { get; set; }
        private const string UserPrintedPrintsColumnName = "USER_PRINTED_PRINTS";
        public double UserPrintHours { get; set; }
        private const string UserPrintHoursColumnName = "USER_PRINT_HOURS";
        public FileResponseDbObject? UserImageData { get; set; }

        public UserDetailDbObject()
        {
            UserImageData = new FileResponseDbObject();
        }

        public UserDetailDbObject Create(DataRow row)
        {
            var obj = new UserDetailDbObject();

            obj.UserId = row.Field<int>(UserIdColumnName);
            obj.UserName = row.Field<string>(UserNameColumnName);
            obj.UserRole = row.Field<string>(UserRoleColumnName);
            obj.UserEmail = row.Field<string>(UserEmailColumnName);
            obj.UserCreateDate = row.Field<DateTime>(UserCreateDateColumnName);
            obj.UserTotalPrints = (int)(row.Field<long?>(UserTotalPrintsColumnName) ?? 0);
            obj.UserTotalHours = (double)(row.Field<decimal?>(UserTotalHoursColumnName) ?? 0);
            obj.UserPrintedPrints = (int)(row.Field<long?>(UserPrintedPrintsColumnName) ?? 0);
            obj.UserPrintHours = (double)(row.Field<decimal?>(UserPrintHoursColumnName) ?? 0);
            obj.UserImageData = UserImageData.Create(row);
            return obj;
        }
    }
}
