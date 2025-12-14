using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Filament
{
    public class CatalogResponseDbObject
    {
        public int Id { get; set; }
        private const string IdColumnName = "ID";
        public string Description { get; set; }
        private const string DescriptionColumnName = "DESCRIPTION";

        public CatalogResponseDbObject Create(DataRow row)
        {
            var obj = new CatalogResponseDbObject();

            obj.Id = row.Field<int>(IdColumnName);
            obj.Description = row.Field<string>(DescriptionColumnName);

            return obj;
        }
    }
}

