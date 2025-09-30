using _3DMANAGER.API.Models;
using _3DMANAGER.BLL.Interfaces;
using _3DMANAGER.BLL.Models;
using _3DMANAGER.DAL.Base;
using _3DMANAGER.DAL.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace _3DMANAGER.BLL.Managers
{
    public class PrinterManager : IPrinterManager
    {
        private IPrinterDbManager _printerDbManager;
        private IMapper _mapper;
        private ILogger<PrinterManager> _logger;
        public PrinterManager(IPrinterDbManager printerDbManager, IMapper mapper, ILogger<PrinterManager> logger)
        {
            _printerDbManager = printerDbManager;
            _mapper = mapper;
            _logger = logger;
        }

        public List<PrinterObject> GetPrinterList(out BaseError error)
        {
            error = null;
            List<PrinterObject> response = null;
            var responseDb = _printerDbManager.GetPrinterList(out ErrorDbObject errorDb);
            if (errorDb != null)
            {
                _logger.LogError("Error al obtener el listado de impresoras");
                error = new BaseError()
                {
                    code = errorDb.code,
                    message = errorDb.message
                };
            }
            response = _mapper.Map<List<PrinterObject>>(responseDb);
            return response;
        }
    }
}
