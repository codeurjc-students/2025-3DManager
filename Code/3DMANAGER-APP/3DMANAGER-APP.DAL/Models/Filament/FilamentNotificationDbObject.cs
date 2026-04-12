using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Filament
{
    public class FilamentNotificationDbObject
    {
        public int FilamentId { get; set; }
        private const string FilamentIdColumnName = "FILAMENT_ID";
        public string FilamentName { get; set; }
        private const string FilamentNameColumnName = "FILAMENT_NAME";
        public int FilamentGroupId { get; set; }
        private const string FilamentGroupIdColumnName = "FILAMENT_GROUP_ID";
        public int OwnerGroupId { get; set; }
        private const string OwnerGroupIdColumnName = "OWNER_GROUP_ID";
        public decimal FilamentLength { get; set; }
        private const string FilamentLengthColumnName = "FILAMENT_LENGTH";


        public FilamentNotificationDbObject Create(DataRow row)
        {
            var obj = new FilamentNotificationDbObject();

            obj.FilamentId = row.Field<int>(FilamentIdColumnName);
            obj.FilamentName = row.Field<string>(FilamentNameColumnName);
            obj.FilamentGroupId = row.Field<int>(FilamentGroupIdColumnName);
            obj.OwnerGroupId = row.Field<int>(OwnerGroupIdColumnName);
            obj.FilamentLength = row.Field<decimal>(FilamentLengthColumnName);

            return obj;
        }
    }
}
