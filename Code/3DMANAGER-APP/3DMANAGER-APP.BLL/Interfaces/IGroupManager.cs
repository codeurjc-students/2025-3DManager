using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Group;

namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IGroupManager
    {
        public bool PostNewGroup(GroupRequest request, out BaseError? error);
    }
}
