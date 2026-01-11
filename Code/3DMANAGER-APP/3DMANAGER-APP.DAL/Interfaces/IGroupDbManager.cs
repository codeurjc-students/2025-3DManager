using _3DMANAGER_APP.DAL.Models.Group;

namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface IGroupDbManager
    {
        public bool PostNewGroup(GroupRequestDbObject request);
        public List<GroupInvitationDbObject> GetGroupInvitations(int userId);
    }
}
