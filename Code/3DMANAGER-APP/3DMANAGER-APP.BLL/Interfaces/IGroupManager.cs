using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Group;

namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IGroupManager
    {
        public bool PostNewGroup(GroupRequest request, out BaseError? error);
        public List<GroupInvitation> GetGroupInvitations(int userId);
        public bool PostAcceptInvitation(int groupId, bool isAccepted, int userId, out BaseError? error);
        public GroupBasicDataResponse GetGroupBasicData(int groupId, out BaseError? error);
        public bool UpdateGroupData(GroupRequest request, int groupId);
        public bool UpdateLeaveGroup(int userId);
        public bool UpdateMembership(int userKickedId);
        public bool DeleteGroup(int userId, int groupId);

    }
}
