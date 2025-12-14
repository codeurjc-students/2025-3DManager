using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.User;

namespace _3DMANAGER_APP.TEST.Models
{

    /// <summary>
    /// Fake DAL CI: a mock response from BBDD
    /// </summary>
    public class FakeUserDbManager : IUserDbManager
    {

        public List<UserListResponseDbObject> GetUserList(int group)
        {
            return new List<UserListResponseDbObject>
            {
                new UserListResponseDbObject
                {
                    UserId = 1,
                    UserName = "user1",
                    UserHours = 3720,
                    UserNumberPrints = 4
                },
                new UserListResponseDbObject
                {
                    UserId = 2,
                    UserName = "user2",
                    UserHours = 780,
                    UserNumberPrints = 3
                }
            };
        }

        public UserDbObject Login(string userName)
        {
            return null;
        }

        public bool PostNewUser(UserCreateRequestDbObject user, out int? error)
        {
            error = null;
            return false;
        }
    }
}
