namespace _3DMANAGER_APP.DAL.Models.Filament
{
    public class FilamentUpdateRequestDbObject
    {
        public int GroupId { get; set; }
        public int FilamentId { get; set; }
        public string FilamentName { get; set; }
        public string? FilamentColor { get; set; }
        public int? FilamentTemperature { get; set; }
        public decimal FilamentLenght { get; set; }
        public string? FilamentDescription { get; set; }
        public decimal FilamentCost { get; set; }
        //public IFormFile? ImageFile { get; set; }
    }

}
