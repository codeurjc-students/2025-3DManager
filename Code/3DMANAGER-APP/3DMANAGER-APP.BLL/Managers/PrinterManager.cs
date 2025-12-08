using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Printer;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Printer;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace _3DMANAGER_APP.BLL.Managers
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

        public bool PostPrinter(PrinterRequest printer, out BaseError? error)
        {
            error = null;

            PrinterRequestDbObject printerDbObject = _mapper.Map<PrinterRequestDbObject>(printer);
            var responseDb = _printerDbManager.PostPrinter(printerDbObject, out int? errorDb);

            if (errorDb != null)
            {
                string msg = "";
                switch (errorDb)
                {
                    case 1:
                        msg = $"Error al crear impresora con nombre {printer.PrinterName}";
                        _logger.LogError(msg);
                        error = new BaseError() { code = StatusCodes.Status409Conflict, message = msg };
                        break;
                    case 500:
                        msg = $"Error al crear impresora en el servidor.";
                        _logger.LogError(msg);
                        error = new BaseError() { code = StatusCodes.Status500InternalServerError, message = msg };
                        break;
                    default:
                        break;
                }

            }
            return responseDb;
        }

        public List<PrinterListObject> GetPrinterDashboardList(int group, out BaseError? error)
        {
            error = null;
            List<PrinterListDbObject> list = _printerDbManager.GetPrinterDashboardList(group);
            if (list == null)
                error = new BaseError() { code = (int)HttpStatusCode.InternalServerError, message = "Error al obtener listado de impresoras" };

            return _mapper.Map<List<PrinterListObject>>(list);
        }
    }
}
