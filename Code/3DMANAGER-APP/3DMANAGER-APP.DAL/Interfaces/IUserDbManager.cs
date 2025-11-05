using _3DMANAGER_APP.DAL.Models.User;

namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface IUserDbManager
    {
        bool PostNewUser(UserCreateRequestDbObject user, out int? error);
        UserDbObject Login(string userName);
    }
}
