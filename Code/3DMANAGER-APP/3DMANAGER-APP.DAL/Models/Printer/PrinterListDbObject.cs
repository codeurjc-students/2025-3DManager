using _3DMANAGER_APP.DAL.Models.File;
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
        public int? PrinterStateId { get; set; }
        private const string PrinterStateIdColumnName = "PRINTER_STATE_ID";
        public string? PrinterStateName { get; set; }
        private const string PrinterStateNameColumnName = "PRINTER_STATE_NAME";
        public FileResponseDbObject PrinterImageData;

        public PrinterListDbObject()
        {
            PrinterImageData = new FileResponseDbObject();
        }

        public PrinterListDbObject Create(DataRow row)
        {
            var obj = new PrinterListDbObject();
            obj.PrinterId = row.Field<int>(PrinterIdColumnName);
            obj.PrinterName = row.Field<string>(PrinterNameColumnName);
            obj.PrinterModel = row.Field<string>(PrinterModelColumnName);
            obj.PrinterDescription = row.Field<string>(PrinterDescriptionColumnName);
            obj.PrinterStateId = row.Field<int>(PrinterStateIdColumnName);
            obj.PrinterStateName = row.Field<string>(PrinterStateNameColumnName);
            obj.PrinterImageData = PrinterImageData.Create(row);
            return obj;
        }
    }
}
