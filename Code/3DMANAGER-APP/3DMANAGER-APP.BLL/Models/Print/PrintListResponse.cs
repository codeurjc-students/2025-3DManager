using _3DMANAGER_APP.BLL.Models.Base;

namespace _3DMANAGER_APP.BLL.Models.Print
{
    public class PrintListResponse : PagedResponse
    {
        public List<PrintResponse> prints { get; set; }
    }
    public class PrintResponse
    {
        public int PrintId { get; set; }
        public string PrintName { get; set; }
        public string PrintUserCreator { get; set; }
        public DateTime PrintDate { get; set; }
        public string PrintTime { get; set; }
        public decimal PrintFilamentConsumed { get; set; }
    }
}
