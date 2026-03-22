using _3DMANAGER_APP.BLL.Models.User;

namespace _3DMANAGER_APP.BLL.Models.Group
{
    public class GroupBasicDataResponse
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public string GroupOwner { get; set; }
        public DateTime GroupDate { get; set; }
        public List<UserListResponse> GroupMembers { get; set; }
    }
}
