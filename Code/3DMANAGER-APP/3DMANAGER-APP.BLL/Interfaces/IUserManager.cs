using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.User;

namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IUserManager
    {
        public bool PostNewUser(UserCreateRequest user, out BaseError? error);
        public UserObject Login(string userName, string userPassword, out BaseError? error);
        public List<UserListResponse> GetUserList(int group, out BaseError? error);
        public List<UserListResponse> GetUserInvitationList(out BaseError? error);
        public void PostUserInvitation(int groupId, int userId);
    }
}
