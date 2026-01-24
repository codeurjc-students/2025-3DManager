namespace _3DMANAGER_APP.DAL.Models.Print
{
    public class PrintRequestDbObject
    {
        public int GroupId { get; set; }
        private const string GroupIdColumnName = "GROUP_ID";
        public int UserId { get; set; }
        private const string UserIdColumnName = "USER_ID";
        public string PrintName { get; set; }
        private const string PrintNameColumnName = "PRINT_NAME";
        public int PrintState { get; set; }
        private const string PrintStateColumnName = "PRINT_STATE";
        public int PrintPrinter { get; set; }
        private const string PrintPrinterColumnName = "PRINT_PRINTER_ID";
        public int PrintFilament { get; set; }
        private const string PrintFilamentColumnName = "PRINT_FILAMENT_ID";
        public string PrintDescription { get; set; }
        private const string PrintDescriptionColumnName = "PRINT_DESCRIPTION";
        public int PrintTime { get; set; }
        private const string PrintTimeColumnName = "PRINT_TIME";
        public decimal PrintFilamentUsed { get; set; }
        private const string PrintFilamentUsedColumnName = "PRINT_FILAMENT_USED";
        public int PrintRealTime { get; set; }
        private const string PrintRealTimeColumnName = "PRINT_REAL_TIME";



    }
}
