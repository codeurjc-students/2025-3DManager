using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Catalog;
using _3DMANAGER_APP.DAL.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace _3DMANAGER_APP.BLL.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository _catalogDbManager;
        private readonly IPrinterRepository _printerDbManager;
        private readonly IMapper _mapper;
        private readonly ILogger<CatalogService> _logger;
        public CatalogService(ICatalogRepository catalogDbManager, IPrinterRepository printerDbManager, IMapper mapper, ILogger<CatalogService> logger)
        {
            _catalogDbManager = catalogDbManager;
            _mapper = mapper;
            _printerDbManager = printerDbManager;
            _logger = logger;
        }

        public List<CatalogResponse> GetFilamentType()
        {
            return _mapper.Map<List<CatalogResponse>>(_catalogDbManager.GetFilamentType());
        }
        public List<CatalogResponse> GetFilamentCatalog(int groupId)
        {
            return _mapper.Map<List<CatalogResponse>>(_catalogDbManager.GetFilamentCatalog(groupId));
        }
        public List<CatalogPrinterResponse> GetPrinterCatalog(int groupId)
        {
            List<CatalogPrinterResponse> list = _mapper.Map<List<CatalogPrinterResponse>>(_catalogDbManager.GetPrinterCatalog(groupId));
            foreach (var printer in list)
            {
                var variation = GetPrinterTimeVariation(groupId, printer.Id, out var error);
                printer.TimeVariation = variation;
            }
            return list;
        }

        public float GetPrinterTimeVariation(int groupId, int printerId, out BaseError? error)
        {
            error = null;
            var responseDb = _printerDbManager.GetTimeVariation(groupId, printerId, out bool errorDb);
            if (errorDb)
            {
                string msg = $"Error al obtener el listado de tiempos de impresión de la impresora {printerId}";
                _logger.LogError(msg);
                error = new BaseError()
                {
                    code = StatusCodes.Status500InternalServerError,
                    message = msg
                };
            }
            var variations = responseDb?.Count > 0 ?
            responseDb.Where(time => time.PrinterTimeImpresion > 0)
            .Average(time => (float)(time.PrinterRealTimeImpresion - time.PrinterTimeImpresion) / time.PrinterTimeImpresion * 100
            ) : 0;

            return variations;
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
