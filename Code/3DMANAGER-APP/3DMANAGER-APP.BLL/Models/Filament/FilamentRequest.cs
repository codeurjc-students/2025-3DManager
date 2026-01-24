namespace _3DMANAGER_APP.BLL.Models.Filament
{
    public class FilamentRequest
    {
        public int GroupId { get; set; }
        public string FilamentName { get; set; }
        public int FilamentType { get; set; }
        public decimal FilamentWeight { get; set; }
        public string FilamentColor { get; set; }
        public int FilamentTemperature { get; set; }
        public decimal FilamentLenght { get; set; }
        public decimal FilamentThickness { get; set; }
        public decimal FilamentCost { get; set; }
        public string FilamentDescription { get; set; }

    }
}
