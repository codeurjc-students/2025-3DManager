using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Printer
{
    public class PrinterTimesValuesDbObject
    {
        public int PrinterTimeImpresion { get; set; }
        private const string PrinterTimeImpresionColumnName = "PRINTER_TIME";
        public int PrinterRealTimeImpresion { get; set; }
        private const string PrinterRealTimeImpresionColumnName = "PRINTER_TIME_REAL";

        public PrinterTimesValuesDbObject Create(DataRow row)
        {
            var obj = new PrinterTimesValuesDbObject();
            obj.PrinterTimeImpresion = row.Field<int>(PrinterTimeImpresionColumnName);
            obj.PrinterRealTimeImpresion = row.Field<int>(PrinterRealTimeImpresionColumnName);
            return obj;
        }
    }
}
