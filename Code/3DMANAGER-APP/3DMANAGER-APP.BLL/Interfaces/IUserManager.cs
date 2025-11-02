using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;

namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IUserManager
    {
        public bool PostNewUser(UserObject user, out BaseError? error);
    }
}
