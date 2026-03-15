namespace _3DMANAGER_APP.DAL.Models.Print
{
    public class PrintDetailRequestDbObject
    {
        public int GroupId { get; set; }
        public int PrintId { get; set; }
        public required string PrintName { get; set; }
        public required string PrintDescription { get; set; }

    }
}
