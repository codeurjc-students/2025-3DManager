using Microsoft.AspNetCore.Http;

namespace _3DMANAGER_APP.BLL.Models.Filament
{
    public class FilamentUpdateRequest
    {
        public int GroupId { get; set; }
        public int FilamentId { get; set; }
        public string FilamentName { get; set; }
        public string? FilamentColor { get; set; }
        public int? FilamentTemperature { get; set; }
        public int FilamentLenght { get; set; }
        public string? FilamentDescription { get; set; }
        public int FilamentCost { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

}
