using _3DMANAGER_APP.BLL.Models.Catalog;

namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface ICatalogManager
    {
        public List<CatalogResponse> GetFilamentType();
    }
}
