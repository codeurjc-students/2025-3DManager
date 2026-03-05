namespace _3DMANAGER_APP.BLL.Models.Print
{
    public class PrintCommentObject
    {
        public int CommentId { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime RegisterDate { get; set; }
    }

}
