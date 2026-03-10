using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Group
{

    public class GroupDashboardDataDbObject
    {
        public int GroupTotalHours { get; set; }
        private const string GroupTotalHoursColumnName = "GROUP_TOTAL_HOURS";
        public int GroupTotalPrints { get; set; }
        private const string GroupTotalPrintsColumnName = "GROUP_TOTAL_PRINTS";
        public decimal GroupTotalFilament { get; set; }
        private const string GroupTotalFilamentColumnName = "GROUP_TOTAL_FILAMENT";
        public int GroupUserCount { get; set; }
        private const string GroupUserCountColumnName = "GROUP_TOTAL_USER";
        public int GroupFilamentCount { get; set; }
        private const string GroupFilamentCountColumnName = "GROUP_TOTAL_FILAMENT";
        public int GroupPrinterCount { get; set; }
        private const string GroupPrinterCountColumnName = "GROUP_TOTAL_PRINTER";
        public List<GroupPrinterHoursDbObject> GroupPrinterHours { get; set; }

        public GroupDashboardDataDbObject()
        {
            GroupPrinterHours = new List<GroupPrinterHoursDbObject>();
        }
        public GroupDashboardDataDbObject Create(DataRow row)
        {
            var obj = new GroupDashboardDataDbObject();

            obj.GroupTotalHours = row.Field<int>(GroupTotalHoursColumnName);
            obj.GroupTotalPrints = row.Field<int>(GroupTotalPrintsColumnName);
            obj.GroupTotalFilament = row.Field<decimal>(GroupTotalFilamentColumnName);
            obj.GroupUserCount = row.Field<int>(GroupUserCountColumnName);
            obj.GroupPrinterCount = row.Field<int>(GroupPrinterCountColumnName);
            obj.GroupFilamentCount = row.Field<int>(GroupFilamentCountColumnName);

            return obj;
        }

    }
}
