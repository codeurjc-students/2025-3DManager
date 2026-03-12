namespace _3DMANAGER_APP.DAL.Models.Print
{
    public class PrintRequestDbObject
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public string PrintName { get; set; }
        public int PrintState { get; set; }
        public int PrintPrinter { get; set; }
        public int PrintFilament { get; set; }
        public string PrintDescription { get; set; }
        public int PrintTime { get; set; }
        public decimal PrintFilamentUsed { get; set; }
        public int PrintRealTime { get; set; }
        public int? PrintProgress { get; set; }



    }
}
