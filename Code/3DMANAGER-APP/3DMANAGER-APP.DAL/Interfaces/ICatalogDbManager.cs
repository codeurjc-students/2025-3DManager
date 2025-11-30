using _3DMANAGER_APP.DAL.Models.Filament;

namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface ICatalogDbManager
    {
        public List<CatalogResponseDbObject> GetFilamentType();
    }
}
