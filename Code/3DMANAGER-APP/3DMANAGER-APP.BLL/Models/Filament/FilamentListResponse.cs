namespace _3DMANAGER_APP.BLL.Models.Filament
{
    public class FilamentListResponse
    {
        public int FilamentId { get; set; }
        public string FilamentName { get; set; }
        public string FilamentState { get; set; }
        public decimal FilamentLength { get; set; }
        public decimal FilamentCost { get; set; }
    }
}
