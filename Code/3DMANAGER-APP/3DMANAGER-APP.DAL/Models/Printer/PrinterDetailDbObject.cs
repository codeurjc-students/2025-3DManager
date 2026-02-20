using _3DMANAGER_APP.DAL.Models.File;
using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Printer
{
    public class PrinterDetailDbObject
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
        public DateTime PrinterCreateDate { get; set; }
        private const string PrinterCreateDateColumnName = "PRINTER_CREATE_DATE";
        public double PrinterTotalHours { get; set; }
        private const string PrinterTotalHoursColumnName = "PRINTER_TOTAL_HOURS";
        public double PrinterTotalHoursMonth { get; set; }
        private const string PrinterTotalHoursMonthColumnName = "PRINTER_TOTAL_HOURS_MONTH";
        public int PrinterPrintsTotal { get; set; }
        private const string PrinterPrintsTotalColumnName = "PRINTER_TOTAL_PRINTS";
        public int PrinterPrintsTotalMonth { get; set; }
        private const string PrinterPrintsTotalMonthColumnName = "PRINTER_TOTAL_PRINTS_MONTH";
        public PrinterEstimationDbObject? PrinterEstimations;
        public FileResponseDbObject? PrinterImageData;

        public PrinterDetailDbObject()
        {
            PrinterImageData = new FileResponseDbObject();
            PrinterEstimations = new PrinterEstimationDbObject();
        }

        public PrinterDetailDbObject Create(DataRow row)
        {
            var obj = new PrinterDetailDbObject();

            obj.PrinterId = row.Field<int>(PrinterIdColumnName);
            obj.PrinterName = row.Field<string>(PrinterNameColumnName);
            obj.PrinterModel = row.Field<string>(PrinterModelColumnName);
            obj.PrinterDescription = row.Field<string>(PrinterDescriptionColumnName);
            obj.PrinterStateId = row.Field<int>(PrinterStateIdColumnName);
            obj.PrinterStateName = row.Field<string>(PrinterStateNameColumnName);
            obj.PrinterCreateDate = row.Field<DateTime>(PrinterCreateDateColumnName);
            obj.PrinterPrintsTotal = (int)(row.Field<long?>(PrinterPrintsTotalColumnName) ?? 0);
            obj.PrinterPrintsTotalMonth = (int)(row.Field<long?>(PrinterPrintsTotalMonthColumnName) ?? 0);
            obj.PrinterTotalHours = (double)(row.Field<decimal?>(PrinterTotalHoursColumnName) ?? 0);
            obj.PrinterTotalHoursMonth = (double)(row.Field<decimal?>(PrinterTotalHoursMonthColumnName) ?? 0);
            obj.PrinterImageData = PrinterImageData.Create(row);
            //obj.PrinterEstimations = PrinterEstimations.Create(row);
            return obj;
        }

    }
}
