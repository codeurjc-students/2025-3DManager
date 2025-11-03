using _3DMANAGER_APP.DAL.Models.User;

namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface IUserDbManager
    {
        bool PostNewUser(UserDbObject user, out int? error);
    }
}
