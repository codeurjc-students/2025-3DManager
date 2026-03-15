using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Catalog;
using _3DMANAGER_APP.DAL.Interfaces;
using AutoMapper;

namespace _3DMANAGER_APP.BLL.Managers
{
    public class CatalogManager : ICatalogManager
    {
        private readonly ICatalogDbManager _catalogDbManager;
        private readonly IMapper _mapper;
        public CatalogManager(ICatalogDbManager catalogDbManager, IMapper mapper)
        {
            _catalogDbManager = catalogDbManager;
            _mapper = mapper;
        }

        public List<CatalogResponse> GetFilamentType()
        {
            return _mapper.Map<List<CatalogResponse>>(_catalogDbManager.GetFilamentType());
        }
        public List<CatalogResponse> GetFilamentCatalog(int groupId)
        {
            return _mapper.Map<List<CatalogResponse>>(_catalogDbManager.GetFilamentCatalog(groupId));
        }
        public List<CatalogResponse> GetPrinterCatalog(int groupId)
        {
            return _mapper.Map<List<CatalogResponse>>(_catalogDbManager.GetPrinterCatalog(groupId));
        }
        public List<CatalogResponse> GetPrintState()
        {
            return _mapper.Map<List<CatalogResponse>>(_catalogDbManager.GetPrintState());
        }
        public List<CatalogResponse> GetPrinterState()
        {
            return _mapper.Map<List<CatalogResponse>>(_catalogDbManager.GetPrinterState());
        }
        public List<CatalogResponse> GetFilamentState()
        {
            return _mapper.Map<List<CatalogResponse>>(_catalogDbManager.GetFilamentState());
        }


    }
}
