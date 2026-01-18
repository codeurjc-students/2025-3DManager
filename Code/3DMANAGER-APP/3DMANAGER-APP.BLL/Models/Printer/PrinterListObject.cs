using _3DMANAGER_APP.BLL.Models.File;

namespace _3DMANAGER_APP.BLL.Models.Printer
{
    public class PrinterListObject
    {
        public int? PrinterId { get; set; }
        public string? PrinterName { get; set; }
        public string? PrinterModel { get; set; }
        public string? PrinterDescription { get; set; }
        public int? PrinterStateId { get; set; }
        public string? PrinterStateName { get; set; }
        public FileResponse? PrinterImageData { get; set; }
    }
}
