namespace _3DMANAGER_APP.DAL.Models.Printer
{
    public class PrinterRequestDbObject
    {
        public int GroupId { get; set; }

        private const string GroupIdColumnName = "P_GROUP_ID";
        public string PrinterName { get; set; }

        private const string PrinterNameColumnName = "P_PRINTER_NAME";
        public string PrinterDescription { get; set; }

        private const string PrinterDescriptionColumnName = "P_PRINTER_DESCRIPTION";
        public string PrinterModel { get; set; }

        private const string PrinterModelwawwwwColumnName = "P_PRINTER_MODEL";
    }
}
