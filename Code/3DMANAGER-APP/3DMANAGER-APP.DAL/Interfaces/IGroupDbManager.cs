using _3DMANAGER_APP.DAL.Models.User;

namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface IGroupDbManager
    {
        public bool PostNewGroup(GroupRequestDbObject request);
    }
}
