namespace _3DMANAGER_APP.BLL.Models.Group
{
    public class PrinterHoursObject
    {
        public int PrinterId { get; set; }
        public string PrinterName { get; set; }
        public string PrinterHours { get; set; }
    }
    public class GroupDashboardData
    {
        public string GroupTotalHours { get; set; }
        public int GroupTotalPrints { get; set; }
        public decimal GroupTotalFilament { get; set; }
        public int GroupUserCount { get; set; }
        public int GroupFilamentCount { get; set; }
        public int GroupPrinterCount { get; set; }
        public List<PrinterHoursObject> GroupPrinterHours { get; set; }

    }
}
