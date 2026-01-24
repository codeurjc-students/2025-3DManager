using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Group
{
    public class GroupInvitationDbObject
    {
        public string UserGroupManager { get; set; }
        private const string UserGroupManagerColumnName = "USER_OWNER";
        public int GroupId { get; set; }
        private const string GroupIdColumnName = "GROUP_ID";

        public string GroupName { get; set; }
        private const string GroupNameColumnName = "GROUP_NAME";
        public string GroupDescription { get; set; }
        private const string GroupDescriptionColumnName = "GROUP_DESCRIPTION";

        public GroupInvitationDbObject Create(DataRow row)
        {
            var obj = new GroupInvitationDbObject();

            obj.UserGroupManager = row.Field<string>(UserGroupManagerColumnName);
            obj.GroupId = row.Field<int>(GroupIdColumnName);
            obj.GroupName = row.Field<string>(GroupNameColumnName);
            obj.GroupDescription = row.Field<string>(GroupDescriptionColumnName);

            return obj;
        }
    }
}
