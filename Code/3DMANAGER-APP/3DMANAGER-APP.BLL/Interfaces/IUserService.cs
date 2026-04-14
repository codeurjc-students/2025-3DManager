using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.User;
using Microsoft.AspNetCore.Http;

namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<CommonResponse<int>> PostNewUser(UserCreateRequest user);
        public UserObject Login(string userName, string userPassword, out BaseError? error);
        public List<UserListResponse> GetUserList(int group, out BaseError? error);
        public List<UserListResponse> GetUserInvitationList(string? filter, out BaseError? error);
        public bool PostUserInvitation(int groupId, int userId, int userOwner, out BaseError? error);
        public UserObject GetUserById(int userId);
        public int GetGroupIdByUserId(int userId);
        bool UpdateUser(UserUpdateRequest request, out BaseError? error);
        UserDetailObject GetUserDetail(int userId, out BaseError? error);
        public Task<CommonResponse<bool>> DeleteUserImage(int userId);
        public Task<CommonResponse<bool>> UpdateUserImage(int userId, IFormFile imageFile);
        public Task<CommonResponse<bool>> DeleteUser(int userId);
    }
}
