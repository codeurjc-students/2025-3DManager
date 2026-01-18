using System.Data;

namespace _3DMANAGER_APP.DAL.Models.File
{
    public class FileResponseDbObject
    {
        public string? FileKey { get; set; }
        private const string KeyColumnName = "FILE_KEY";
        public string? FileUrl { get; set; }
        private const string UrlColumnName = "FILE_URL";

        public FileResponseDbObject Create(DataRow row)
        {
            var obj = new FileResponseDbObject();
            obj.FileKey = row.Field<string?>(KeyColumnName);
            obj.FileUrl = row.Field<string?>(UrlColumnName);
            return obj;
        }
    }
}
