using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Group
{

    public class GroupDashboardDataDbObject
    {
        public double GroupTotalHours { get; set; }
        private const string GroupTotalHoursColumnName = "GROUP_TOTAL_HOURS";
        public int GroupTotalPrints { get; set; }
        private const string GroupTotalPrintsColumnName = "GROUP_TOTAL_PRINTS";
        public decimal GroupTotalFilament { get; set; }
        private const string GroupTotalFilamentColumnName = "GROUP_TOTAL_FILAMENT";
        public int GroupUserCount { get; set; }
        private const string GroupUserCountColumnName = "GROUP_TOTAL_USER";
        public int GroupFilamentCount { get; set; }
        private const string GroupFilamentCountColumnName = "GROUP_FILAMENT_COUNT";
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

            obj.GroupTotalHours = (double)(row.Field<decimal?>(GroupTotalHoursColumnName) ?? 0m);
            obj.GroupTotalPrints = (int)row.Field<long>(GroupTotalPrintsColumnName);
            obj.GroupTotalFilament = row.Field<decimal?>(GroupTotalFilamentColumnName) ?? 0;
            obj.GroupUserCount = (int)row.Field<long>(GroupUserCountColumnName);
            obj.GroupPrinterCount = (int)row.Field<long>(GroupPrinterCountColumnName);
            obj.GroupFilamentCount = (int)row.Field<long>(GroupFilamentCountColumnName);

            return obj;
        }

    }
}
