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
        public decimal FilamentLength { get; set; }
        private const string FilamentLengthColumnName = "FILAMENT_LENGTH";
        public decimal FilamentCost { get; set; }
        private const string FilamentCostColumnName = "FILAMENT_COST";

        public FilamentListResponseDbObject Create(DataRow row)
        {
            var obj = new FilamentListResponseDbObject();

            obj.FilamentId = row.Field<int>(FilamentIdColumnName);
            obj.FilamentName = row.Field<string>(FilamentNameColumnName);
            obj.FilamentState = row.Field<string>(FilamentStateColumnName);
            obj.FilamentLength = row.Field<decimal>(FilamentLengthColumnName);
            obj.FilamentCost = row.Field<decimal>(FilamentCostColumnName);

            return obj;
        }
    }
}

