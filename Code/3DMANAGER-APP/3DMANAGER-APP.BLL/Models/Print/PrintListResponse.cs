namespace _3DMANAGER_APP.BLL.Models.Print
{
    public class PrintListResponse
    {
        public int PrintId { get; set; }
        public string PrintName { get; set; }
        public string PrintUserCreator { get; set; }
        public DateTime PrintDate { get; set; }
        public decimal PrintTime { get; set; }
        public decimal PrintFilamentConsumed { get; set; }
    }
}
