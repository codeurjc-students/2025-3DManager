using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Printer
{
    public class PrinterEstimationDbObject
    {
        public float SuccessRate { get; set; }
        private const string SuccessRateColumnName = "PRINTER_SUCCESS_RATE";
        public float TimeVariation { get; set; }
        private const string TimeVariationColumnName = "PRINTER_TIME_VARIATION";

        public PrinterEstimationDbObject Create(DataRow row)
        {
            var obj = new PrinterEstimationDbObject();
            obj.SuccessRate = row.Field<float>(SuccessRateColumnName);
            obj.TimeVariation = row.Field<float>(TimeVariationColumnName);
            return obj;
        }
    }
}
