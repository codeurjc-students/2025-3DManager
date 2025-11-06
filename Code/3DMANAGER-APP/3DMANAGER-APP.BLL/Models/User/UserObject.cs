namespace _3DMANAGER_APP.BLL.Models.User
{
    public class UserObject
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string? UserEmail { get; set; }
        public int? GroupId { get; set; }
        public string? RolId { get; set; }
    }
}
