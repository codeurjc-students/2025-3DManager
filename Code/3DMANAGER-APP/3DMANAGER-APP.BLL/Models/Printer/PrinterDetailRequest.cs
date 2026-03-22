namespace _3DMANAGER_APP.BLL.Models.Printer
{
    public class PrinterDetailRequest
    {
        public int GroupId { get; set; }
        public int PrinterId { get; set; }
        public string PrinterName { get; set; }
        public string PrinterDescription { get; set; }
        public string PrinterModel { get; set; }

        public int PrinterStateId { get; set; }
        //public IFormFile? ImageFile { get; set; }

    }
}
