using _3DMANAGER_APP.DAL.Models.File;
using _3DMANAGER_APP.DAL.Models.User;

namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface IUserDbManager
    {
        public int PostNewUser(UserCreateRequestDbObject user, out int? error);
        public UserDbObject Login(string userName);
        public List<UserListResponseDbObject> GetUserList(int group);
        public List<UserListResponseDbObject> GetUserInvitationList();
        public void PostUserInvitation(int groupId, int userId);
        public bool UpdateUserImageData(int userId, FileResponseDbObject image);
    }
}
