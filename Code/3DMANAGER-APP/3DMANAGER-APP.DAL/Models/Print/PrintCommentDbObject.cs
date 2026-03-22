using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Print
{
    public class PrintCommentDbObject
    {
        public int CommentId { get; set; }
        private const string CommentIdColumnName = "ID_COMMENT";
        public string Comment { get; set; }
        private const string CommentColumnName = "DS_COMMENT";
        public int UserId { get; set; }
        private const string UserIdColumnName = "COMMENT_USER_ID";
        public string UserName { get; set; }
        private const string UserNameColumnName = "COMMENT_USER_NAME";
        public DateTime RegisterDate { get; set; }
        private const string RegisterDateColumnName = "COMMENT_DATE";


        public PrintCommentDbObject Create(DataRow row)
        {
            var obj = new PrintCommentDbObject();
            obj.CommentId = row.Field<int>(CommentIdColumnName);
            obj.Comment = row.Field<string>(CommentColumnName);
            obj.UserId = row.Field<int>(UserIdColumnName);
            obj.UserName = row.Field<string>(UserNameColumnName);
            obj.RegisterDate = row.Field<DateTime>(RegisterDateColumnName);
            return obj;
        }

    }

}
