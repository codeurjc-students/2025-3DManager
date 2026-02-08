using _3DMANAGER_APP.DAL.Models.Group;

namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface IGroupDbManager
    {
        public bool PostNewGroup(GroupRequestDbObject request);
        public List<GroupInvitationDbObject> GetGroupInvitations(int userId);
        public bool PostAcceptInvitation(int groupId, bool isAccepted, int userId, out int? errorDb);
        public GroupBasicDataResponseDbObject GetGroupBasicData(int groupId);
        public bool UpdateGroupData(GroupRequestDbObject request, int groupId);
    }
}
