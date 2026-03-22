using _3DMANAGER_APP.BLL.Models.File;

namespace _3DMANAGER_APP.BLL.Models.User
{
    public class UserDetailObject
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string userRole { get; set; }
        public string userEmail { get; set; }
        public DateTime UserCreateDate { get; set; }
        public int UserTotalPrints { get; set; }
        public string UserTotalHours { get; set; }
        public int UserPrintedPrints { get; set; }
        public string UserPrintHours { get; set; }
        public FileResponse? UserImageData { get; set; }
    }
}
