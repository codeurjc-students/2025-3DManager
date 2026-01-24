using _3DMANAGER_APP.DAL.Models.Filament;

namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface IFilamentDbManager
    {
        public List<FilamentListResponseDbObject> GetFilamentList(int group);
        public bool PostFilament(FilamentRequestDbObject request, out int? error);
    }
}
