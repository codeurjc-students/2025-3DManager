using _3DMANAGER_APP.DAL.Models.File;
using _3DMANAGER_APP.DAL.Models.Print;
using _3DMANAGER_APP.DAL.Models.User;


namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface IUserDbManager
    {
        public int PostNewUser(UserCreateRequestDbObject user, out int? error);
        public UserDbObject Login(string userName);
        public List<UserListResponseDbObject> GetUserList(int group, out bool error);
        public List<UserListResponseDbObject> GetUserInvitationList(string? filter, out bool error);
        public bool PostUserInvitation(int groupId, int userId, out int? error);
        public bool UpdateUserImageData(int userId, FileResponseDbObject image);
        public UserDbObject GetUserById(int userId);
        public int GetGroupIdByUserId(int userId);
        bool UpdateUser(UserUpdateRequestDbObject requestDb, out int? error);
        UserDetailDbObject GetUserDetail(int userId);
        public FileResponseDbObject GetUserImageData(int userId, out bool error);
        public bool DeleteUserImageData(int userId);
        DeletedDbObject DeleteUser(int userId, out int? error);

    }
}
