using System.Data;

namespace _3DMANAGER_APP.DAL.Models.User
{
    public class UserListResponseDbObject
    {
        public int UserId { get; set; }
        private const string UserIdColumnName = "USER_ID";
        public string UserName { get; set; }
        private const string UserNameColumnName = "USER_NAME";
        public decimal UserHours { get; set; }
        private const string UserHoursColumnName = "USER_HOURS";
        public int UserNumberPrints { get; set; }
        private const string UserNumberPrintsColumnName = "NUMBER_PRINTS";

        public UserListResponseDbObject Create(DataRow row)
        {
            var obj = new UserListResponseDbObject();

            obj.UserId = row.Field<int>(UserIdColumnName);
            obj.UserName = row.Field<string>(UserNameColumnName);
            obj.UserHours = row.Table.Columns.Contains(UserHoursColumnName) ? row.Field<decimal>(UserHoursColumnName) : 0;
            obj.UserNumberPrints = row.Table.Columns.Contains(UserHoursColumnName) ? (int)row.Field<long>(UserNumberPrintsColumnName) : 0;

            return obj;
        }
    }
}
