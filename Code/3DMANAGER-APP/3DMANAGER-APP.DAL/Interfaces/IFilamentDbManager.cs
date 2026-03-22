using _3DMANAGER_APP.DAL.Models.Filament;
using _3DMANAGER_APP.DAL.Models.File;
using _3DMANAGER_APP.DAL.Models.Print;

namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface IFilamentDbManager
    {
        public List<FilamentListResponseDbObject> GetFilamentList(int group);
        public int PostFilament(FilamentRequestDbObject request, out int? error);
        public bool UpdateFilamentImageData(int filamentId, FileResponseDbObject image);
        public bool UpdateFilament(FilamentUpdateRequestDbObject requestDb);
        FilamentDetailDbObject GetFilamentDetail(int groupId, int filamentId);
        DeletedDbObject DeleteFilament(int filamentId, int groupId, out int? error);
        public FileResponseDbObject GetFilamentImageData(int filamentId, int groupId, out bool error);
        public bool DeleteFilamentImageData(int filamentId, int groupId);
    }
}
