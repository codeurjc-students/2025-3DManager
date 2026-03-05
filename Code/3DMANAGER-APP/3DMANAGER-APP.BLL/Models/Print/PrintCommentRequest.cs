namespace _3DMANAGER_APP.BLL.Models.Print
{
    public class PrintCommentRequest
    {
        public int PrintId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
    }

}
