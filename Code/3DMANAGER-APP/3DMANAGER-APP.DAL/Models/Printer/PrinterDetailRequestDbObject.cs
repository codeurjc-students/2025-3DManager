namespace _3DMANAGER_APP.DAL.Models.Printer
{
    public class PrinterDetailRequestDbObject
    {
        public int? GroupId { get; set; }
        public int? PrinterId { get; set; }
        public string? PrinterName { get; set; }
        public string? PrinterModel { get; set; }
        public string? PrinterDescription { get; set; }
        public int? PrinterStateId { get; set; }

        //public FileResponseDbObject? PrinterImageData;

        //public PrinterDetailRequestDbObject()
        //{
        //    PrinterImageData = new FileResponseDbObject();
        //}

    }
}
