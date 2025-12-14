using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Filament;

namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IFilamentManager
    {
        public List<FilamentListResponse> GetFilamentList(int group, out BaseError? error);
        public bool PostFilament(FilamentRequest filament, out BaseError? error);
    }
}
