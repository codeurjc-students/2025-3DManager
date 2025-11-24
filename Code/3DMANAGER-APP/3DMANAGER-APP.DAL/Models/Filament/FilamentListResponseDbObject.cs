using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Filament
{
    public class FilamentListResponseDbObject
    {
        public int FilamentId { get; set; }
        private const string FilamentIdColumnName = "FILAMENT_ID";
        public string FilamentName { get; set; }
        private const string FilamentNameColumnName = "FILAMENT_NAME";
        public string FilamentState { get; set; }
        private const string FilamentStateColumnName = "FILAMENT_STATE";
        public decimal FilamentConsumed { get; set; }
        private const string FilamentConsumedColumnName = "FILAMENT_CONSUMED";
        public int FilamentPrints { get; set; }
        private const string FilamentPrintsColumnName = "FILAMENT_NUMBER_PRINT";

        public FilamentListResponseDbObject Create(DataRow row)
        {
            var obj = new FilamentListResponseDbObject();

            obj.FilamentId = row.Field<int>(FilamentIdColumnName);
            obj.FilamentName = row.Field<string>(FilamentNameColumnName);
            obj.FilamentState = row.Field<string>(FilamentStateColumnName);
            obj.FilamentConsumed = row.Field<decimal>(FilamentConsumedColumnName);
            obj.FilamentPrints = row.Field<int>(FilamentPrintsColumnName);

            return obj;
        }
    }
}

