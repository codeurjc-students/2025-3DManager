using _3DMANAGER_APP.DAL.Models.Filament;
using _3DMANAGER_APP.DAL.Models.File;

namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface IFilamentDbManager
    {
        public List<FilamentListResponseDbObject> GetFilamentList(int group);
        public int PostFilament(FilamentRequestDbObject request, out int? error);
        public bool UpdateFilamentImageData(int filamentId, FileResponseDbObject image);
    }
}
