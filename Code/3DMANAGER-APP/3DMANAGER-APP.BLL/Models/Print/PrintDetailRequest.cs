namespace _3DMANAGER_APP.BLL.Models.Print
{
    public class PrintDetailRequest
    {
        public int GroupId { get; set; }
        public int PrintId { get; set; }
        public string PrintName { get; set; }
        public string PrintDescription { get; set; }
        public int PrintRealTime { get; set; }

    }
}
