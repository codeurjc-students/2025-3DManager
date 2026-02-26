using _3DMANAGER_APP.BLL.Models.File;

namespace _3DMANAGER_APP.BLL.Models.Filament
{
    public class FilamentDetailObject
    {
        public int FilamentId { get; set; }
        public string FilamentName { get; set; }
        public string FilamentType { get; set; }
        public int FilamentWeight { get; set; }
        public string FilamentColor { get; set; }
        public int FilamentTemperature { get; set; }
        public decimal FilamentLenght { get; set; }
        public decimal FilamentRemainingLenght { get; set; }
        public float FilamentThickness { get; set; }
        public decimal FilamentCost { get; set; }
        public string FilamentDescription { get; set; }
        public DateTime FilamentCreateDate { get; set; }
        public int FilamentState { get; set; }
        public int FilamentPrintedPrintsMonth { get; set; }
        public int FilamentPrintedPrintsTotal { get; set; }
        public FileResponse? FilamentImageFile { get; set; }

    }
}
