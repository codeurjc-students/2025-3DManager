using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Printer
{
    public class PrinterListDbObject
    {
        public int? PrinterId { get; set; }
        private const string PrinterIdColumnName = "PRINTER_ID";
        public string? PrinterName { get; set; }
        private const string PrinterNameColumnName = "PRINTER_NAME";
        public string? PrinterModel { get; set; }
        private const string PrinterModelColumnName = "PRINTER_MODEL";
        public string? PrinterDescription { get; set; }
        private const string PrinterDescriptionColumnName = "PRINTER_DESCRIPTION";


        public PrinterListDbObject Create(DataRow row)
        {
            var obj = new PrinterListDbObject();
            obj.PrinterId = row.Field<int>(PrinterIdColumnName);
            obj.PrinterName = row.Field<string>(PrinterNameColumnName);
            obj.PrinterModel = row.Field<string>(PrinterModelColumnName);
            obj.PrinterDescription = row.Field<string>(PrinterDescriptionColumnName);

            return obj;
        }
    }
}
