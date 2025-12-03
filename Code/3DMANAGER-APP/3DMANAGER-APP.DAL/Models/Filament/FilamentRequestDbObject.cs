namespace _3DMANAGER_APP.DAL.Models.Filament
{
    public class FilamentRequestDbObject
    {
        public int GroupId { get; set; }
        private const string GroupIdColumnName = "P_GROUP_ID";
        public string FilamentName { get; set; }
        private const string FilamentNameColumnName = "P_FILAMENT_NAME";
        public int FilamentType { get; set; }
        private const string FilamentTypeColumnName = "P_FILAMENT_TYPE";
        public decimal FilamentWeight { get; set; }
        private const string FilamentWeightColumnName = "P_FILAMENT_WEIGHT";
        public string FilamentColor { get; set; }
        private const string FilamentColorColumnName = "P_FILAMENT_COLOR";
        public int FilamentTemperature { get; set; }
        private const string FilamentTemperatureColumnName = "P_FILAMENT_TEMPERATURE";
        public int FilamentLenght { get; set; }
        private const string FilamentLenghtColumnName = "P_FILAMENT_LENGHT";
        public decimal FilamentThickness { get; set; }
        private const string FilamentThicknessColumnName = "P_FILAMENT_THICKNESS";
        public decimal FilamentCost { get; set; }
        private const string FilamentCostColumnName = "P_FILAMENT_COST";
        public string FilamentDescription { get; set; }
        private const string FilamentDescriptionColumnName = "P_FILAMENT_DESCRIPTION";


    }
}

