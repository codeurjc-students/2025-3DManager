using _3DMANAGER_APP.DAL.Models.User;
using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Group
{
    public class GroupBasicDataResponseDbObject
    {
        public int GroupId { get; set; }
        private const string GroupIdColumnName = "GROUP_ID";
        public string GroupName { get; set; }
        private const string GroupNameColumnName = "GROUP_NAME";
        public string GroupDescription { get; set; }
        private const string GroupDescriptionColumnName = "GROUP_DESCRIPTION";
        public string GroupOwner { get; set; }
        private const string GroupOwnerColumnName = "GROUP_OWNER";
        public DateTime GroupDate { get; set; }
        private const string GroupDateColumnName = "GROUP_DATE";
        public List<UserListResponseDbObject> GroupMembers { get; set; }

        public GroupBasicDataResponseDbObject()
        {
            GroupMembers = new List<UserListResponseDbObject>();
        }

        public GroupBasicDataResponseDbObject Create(DataRow row)
        {
            var obj = new GroupBasicDataResponseDbObject();

            obj.GroupId = row.Field<int>(GroupIdColumnName);
            obj.GroupName = row.Field<string>(GroupNameColumnName);
            obj.GroupDescription = row.Field<string>(GroupDescriptionColumnName);
            obj.GroupOwner = row.Field<string>(GroupOwnerColumnName);
            obj.GroupDate = row.Field<DateTime>(GroupDateColumnName);

            return obj;
        }

    }
}
