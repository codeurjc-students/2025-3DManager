using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Group
{
    public class GroupPrinterHoursDbObject
    {
        public int PrinterId { get; set; }
        private const string PrinterIdColumnName = "PRINTER_ID";
        public string PrinterName { get; set; }
        private const string PrinterNameColumnName = "PRINTER_NAME";
        public decimal PrinterHours { get; set; }
        private const string PrinterHoursColumnName = "PRINTER_HOURS";

        public GroupPrinterHoursDbObject Create(DataRow row)
        {
            var obj = new GroupPrinterHoursDbObject();

            obj.PrinterId = row.Field<int>(PrinterIdColumnName);
            obj.PrinterName = row.Field<string>(PrinterNameColumnName);
            obj.PrinterHours = row.Field<decimal>(PrinterHoursColumnName);

            return obj;
        }
    }
}
