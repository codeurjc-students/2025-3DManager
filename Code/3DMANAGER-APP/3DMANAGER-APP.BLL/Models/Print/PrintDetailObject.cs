using _3DMANAGER_APP.BLL.Models.File;

namespace _3DMANAGER_APP.BLL.Models.Print
{
    public class PrintDetailObject
    {
        public int PrintId { get; set; }
        public int PrintFilamentId { get; set; }
        public string PrintFilamentName { get; set; }
        public string PrintMaterial { get; set; }
        public int PrintPrinterId { get; set; }
        public string PrintPrinterName { get; set; }
        public int PrintUserId { get; set; }
        public string PrintUserName { get; set; }
        public string PrintName { get; set; }
        public int PrintState { get; set; }
        public string? PrintDescription { get; set; }
        public FileResponse? PrintImageData { get; set; }
        public DateTime PrintCreateDate { get; set; }
        public decimal PrintMaterialConsumed { get; set; }
        public string PrintTimeImpression { get; set; }
        public string PrintRealTimeImpression { get; set; }
        public decimal PrintEstimedCost { get; set; }
    }
}
