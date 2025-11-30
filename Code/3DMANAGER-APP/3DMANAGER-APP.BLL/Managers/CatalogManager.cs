using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Catalog;
using _3DMANAGER_APP.DAL.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace _3DMANAGER_APP.BLL.Managers
{
    public class CatalogManager : ICatalogManager
    {
        private readonly ICatalogDbManager _catalogDbManager;
        private readonly IMapper _mapper;
        private readonly ILogger<CatalogManager> _logger;
        public CatalogManager(ICatalogDbManager catalogDbManager, IMapper mapper, ILogger<CatalogManager> logger)
        {
            _catalogDbManager = catalogDbManager;
            _mapper = mapper;
            _logger = logger;
        }

        public List<CatalogResponse> GetFilamentType()
        {
            return _mapper.Map<List<CatalogResponse>>(_catalogDbManager.GetFilamentType());
        }
    }
}
