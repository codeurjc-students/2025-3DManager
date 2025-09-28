using _3DMANAGER.API.Models;
using _3DMANAGER.BLL.Interfaces;
using _3DMANAGER.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace _3DMANAGER.API.Controllers
{
    public class PrinterController : BaseController
    {
        private readonly IPrinterManager _printerManager;

        public PrinterController(IPrinterManager printerManager, ILogger<PrinterController> logger) : base(logger)
        {
            _printerManager = printerManager;
        }


        /// <summary>
        /// Create a simple fuction to get the list os printers
        /// </summary>
        /// <returns>Name of the printers</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        
        [HttpGet]
        public IActionResult GetPrinterList()
        {
            _logger.LogInformation("Calling GetPrinterList function");
            List<PrinterObject> response = _printerManager.GetPrinterList();
            return Ok(new CommonResponse<List<PrinterObject>>(response));
            
        }
    }
}
