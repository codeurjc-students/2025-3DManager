namespace _3DMANAGER_APP.BLL.Models.User
{
    public class UserUpdateRequest
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }

    }
}
