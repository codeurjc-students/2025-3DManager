using _3DMANAGER_APP.DAL.Models.File;
using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Filament
{
    public class FilamentDetailDbObject
    {

        public int FilamentId { get; set; }
        private const string FilamentIdColumnName = "FILAMENT_ID";
        public string FilamentName { get; set; }
        private const string FilamentNameColumnName = "FILAMENT_NAME";
        public string FilamentType { get; set; }
        private const string FilamentTypeColumnName = "FILAMENT_TYPE";
        public int FilamentWeight { get; set; }
        private const string FilamentWeightColumnName = "FILAMENT_WEIGHT";
        public string? FilamentColor { get; set; }
        private const string FilamentColorColumnName = "FILAMENT_COLOR";
        public int? FilamentTemperature { get; set; }
        private const string FilamentTemperatureColumnName = "FILAMENT_TEMPERATURE";
        public decimal FilamentLenght { get; set; }
        private const string FilamentLenghtColumnName = "FILAMENT_LENGHT";
        public decimal FilamentRemainingLenght { get; set; }
        private const string FilamentRemainingLenghtColumnName = "FILAMENT_REMAINING_LENGHT";
        public float? FilamentThickness { get; set; }
        private const string FilamentThicknessColumnName = "FILAMENT_THICKNESS";
        public decimal FilamentCost { get; set; }
        private const string FilamentCostColumnName = "FILAMENT_COST";
        public string? FilamentDescription { get; set; }
        private const string FilamentDescriptionColumnName = "FILAMENT_DESCRIPTION";
        public DateTime FilamentCreateDate { get; set; }
        private const string FilamentCreateDateColumnName = "FILAMENT_CREATE_DATE";
        public int FilamentState { get; set; }
        private const string FilamentStateColumnName = "FILAMENT_STATE";
        public int FilamentPrintedPrintsMonth { get; set; }
        private const string FilamentPrintedPrintsMonthColumnName = "FILAMENT_PRINTED_PRINTS_MONTH";
        public int FilamentPrintedPrintsTotal { get; set; }
        private const string FilamentPrintedPrintsTotalColumnName = "FILAMENT_PRINTED_PRINTS_TOTAL";
        public FileResponseDbObject? FilamentImageFile { get; set; }


        public FilamentDetailDbObject()
        {
            FilamentImageFile = new FileResponseDbObject();
        }

        public FilamentDetailDbObject Create(DataRow row)
        {
            var obj = new FilamentDetailDbObject();

            obj.FilamentId = row.Field<int>(FilamentIdColumnName);
            obj.FilamentName = row.Field<string>(FilamentNameColumnName);
            obj.FilamentType = row.Field<string>(FilamentTypeColumnName);
            obj.FilamentWeight = row.Field<int>(FilamentWeightColumnName);
            obj.FilamentColor = row.Field<string>(FilamentColorColumnName);
            obj.FilamentTemperature = row.Field<int>(FilamentTemperatureColumnName);
            obj.FilamentCreateDate = row.Field<DateTime>(FilamentCreateDateColumnName);
            obj.FilamentLenght = row.Field<decimal>(FilamentLenghtColumnName);
            obj.FilamentRemainingLenght = row.Field<decimal>(FilamentRemainingLenghtColumnName);
            obj.FilamentThickness = row.Field<float>(FilamentThicknessColumnName);
            obj.FilamentCost = row.Field<decimal>(FilamentCostColumnName);
            obj.FilamentDescription = row.Field<string>(FilamentDescriptionColumnName);
            obj.FilamentState = row.Field<int>(FilamentStateColumnName);
            obj.FilamentPrintedPrintsMonth = (int)row.Field<long>(FilamentPrintedPrintsMonthColumnName);
            obj.FilamentPrintedPrintsTotal = (int)row.Field<long>(FilamentPrintedPrintsTotalColumnName);
            obj.FilamentImageFile = FilamentImageFile.Create(row);
            return obj;
        }
    }
}
