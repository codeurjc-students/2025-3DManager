using _3DMANAGER_APP.BLL.Models.File;

namespace _3DMANAGER_APP.BLL.Models.Print
{
    public class PrintDetailObject
    {
        public int PrintId { get; set; }
        public int PrintFilamentId { get; set; }
        public string PrintMaterial { get; set; }
        public int PrintPrinterId { get; set; }
        public int PrintUserId { get; set; }
        public string PrintName { get; set; }
        public int PrintState { get; set; }
        public string? PrinterDescription { get; set; }
        public FileResponse? PrintImageData { get; set; }
        public DateTime PrintCreateDate { get; set; }
        public decimal PrintMaterialConsumed { get; set; }
        public int PrintTimeImpression { get; set; }
        public int PrintRealTimeImpression { get; set; }
        public decimal PrintEstimedCost { get; set; }
    }
}
