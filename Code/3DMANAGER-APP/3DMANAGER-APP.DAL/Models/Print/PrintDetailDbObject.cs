using _3DMANAGER_APP.DAL.Models.File;
using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Print
{
    public class PrintDetailDbObject
    {
        public int PrintId { get; set; }
        private const string PrintIdColumnName = "PRINT_ID";
        public int PrintFilamentId { get; set; }
        private const string PrintFilamentIdColumnName = "PRINT_FILAMENT_ID";
        public int PrintFilamentName { get; set; }
        private const string PrintFilamentNameColumnName = "PRINT_FILAMENT_NAME";
        public string PrintMaterial { get; set; }
        private const string PrintMaterialColumnName = "PRINT_MATERIAL";
        public int PrintPrinterId { get; set; }
        private const string PrintPrinterIdColumnName = "PRINT_PRINTER_ID";
        public int PrintPrintName { get; set; }
        private const string PrintPrinterNameColumnName = "PRINT_PRINTER_NAME";
        public int PrintUserId { get; set; }
        private const string PrintUserIdColumnName = "PRINT_USER_ID";
        public int PrintUserName { get; set; }
        private const string PrintUserNameColumnName = "PRINT_USER_NAME";
        public string PrintName { get; set; }
        private const string PrintNameColumnName = "PRINT_NAME";
        public int PrintState { get; set; }
        private const string PrintStateColumnName = "PRINT_STATE";
        public string? PrintDescription { get; set; }
        private const string PrintDescriptionColumnName = "PRINT_DESCRIPTION";
        public FileResponseDbObject? PrintImageData { get; set; }
        public DateTime PrintCreateDate { get; set; }
        private const string PrintCreateDateColumnName = "PRINT_DATE_CREATE_TIME";
        public decimal PrintMaterialConsumed { get; set; }
        private const string PrintMaterialConsumedColumnName = "PRINT_MATERIAL_CONSUMED";
        public int PrintTimeImpression { get; set; }
        private const string PrintTimeImpressionColumnName = "PRINT_TIME_IMPRESSION";
        public int PrintRealTimeImpression { get; set; }
        private const string PrintRealTimeImpressionColumnName = "PRINT_REAL_TIME_IMPRESSION";
        public decimal FilamentCost { get; set; }
        private const string FilamentCostColumnName = "FILAMENT_COST";

        public PrintDetailDbObject Create(DataRow row)
        {
            var obj = new PrintDetailDbObject();

            obj.PrintId = row.Field<int>(PrintIdColumnName);
            obj.PrintName = row.Field<string>(PrintNameColumnName);
            obj.PrintFilamentId = row.Field<int>(PrintFilamentIdColumnName);
            obj.PrintFilamentName = row.Field<int>(PrintFilamentNameColumnName);
            obj.PrintPrinterId = row.Field<int>(PrintPrinterIdColumnName);
            obj.PrintPrintName = row.Field<int>(PrintPrinterNameColumnName);
            obj.PrintUserId = row.Field<int>(PrintUserIdColumnName);
            obj.PrintUserName = row.Field<int>(PrintUserNameColumnName);
            obj.PrintDescription = row.Field<string>(PrintDescriptionColumnName);
            obj.PrintState = row.Field<int>(PrintStateColumnName);
            obj.PrintCreateDate = row.Field<DateTime>(PrintCreateDateColumnName);
            obj.PrintMaterial = row.Field<string>(PrintMaterialColumnName);
            obj.PrintMaterialConsumed = row.Field<decimal>(PrintMaterialConsumedColumnName);
            obj.FilamentCost = row.Field<decimal>(FilamentCostColumnName);
            obj.PrintTimeImpression = row.Field<int>(PrintTimeImpressionColumnName);
            obj.PrintRealTimeImpression = row.Field<int>(PrintRealTimeImpressionColumnName);
            obj.PrintImageData = PrintImageData.Create(row);
            return obj;
        }
    }
}
