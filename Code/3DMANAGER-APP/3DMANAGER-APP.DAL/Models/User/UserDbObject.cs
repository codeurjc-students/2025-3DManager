using System.Data;

namespace _3DMANAGER_APP.DAL.Models.User
{
    public class UserDbObject
    {
        public int UserId { get; set; }
        private const string UserIdColumnName = "3DMANAGER_USER_ID";
        public string UserName { get; set; }
        private const string UserNameColumnName = "3DMANAGER_USER_NAME";
        public string UserPassword { get; set; }
        private const string UserPasswordColumnName = "3DMANAGER_USER_PASSWORD";
        public string? UserEmail { get; set; }
        private const string UserEmailColumnName = "3DMANAGER_USER_EMAIL";
        public int? GroupId { get; set; }
        private const string GroupIdColumnName = "3DMANAGER_USER_GROUP_ID";
        public string? RolId { get; set; }
        private const string RolIdColumnName = "USER_ROLE";
        public string? GroupName { get; set; }
        private const string GroupNameColumnName = "GROUP_NAME";



        public static UserDbObject Create(DataRow row)
        {
            var obj = new UserDbObject();

            obj.UserId = row.Field<int>(UserIdColumnName);
            obj.UserName = row.Field<string>(UserNameColumnName);
            obj.UserPassword = row.Field<string>(UserPasswordColumnName);
            obj.UserEmail = row.Field<string>(UserEmailColumnName);
            obj.GroupId = row.Field<int?>(GroupIdColumnName);
            obj.RolId = row.Field<string?>(RolIdColumnName);
            obj.GroupName = row.Field<string?>(GroupNameColumnName);

            return obj;
        }
    }
}
