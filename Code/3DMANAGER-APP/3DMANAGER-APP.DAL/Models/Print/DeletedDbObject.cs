using _3DMANAGER_APP.DAL.Models.File;
using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Print
{
    public class DeletedDbObject
    {
        public int Id { get; set; }
        private const string IdColumnName = "ID";
        public bool SuccesfullDelete { get; set; }
        public FileResponseDbObject FileResponse { get; set; }

        public DeletedDbObject()
        {
            FileResponse = new FileResponseDbObject();
        }
        public DeletedDbObject Create(DataRow row)
        {
            var obj = new DeletedDbObject();

            obj.Id = row.Field<int>(IdColumnName);
            obj.FileResponse = FileResponse.Create(row);
            return obj;
        }
    }
}
