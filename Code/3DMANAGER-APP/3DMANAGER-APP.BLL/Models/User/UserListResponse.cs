namespace _3DMANAGER_APP.BLL.Models.User
{
    public class UserListResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public decimal UserHours { get; set; }
        public int UserNumberPrints { get; set; }
    }
}
