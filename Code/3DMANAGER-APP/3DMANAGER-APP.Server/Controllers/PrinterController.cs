using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.Server.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace _3DMANAGER_APP.Server.Controllers
{
    [ApiController]
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
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommonResponse<List<PrinterObject>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse<List<PrinterObject>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Printer")]
        [HttpGet]
        public IActionResult GetPrinterList()
        {
            _logger.LogInformation("Calling GetPrinterList function");
            List<PrinterObject> response = _printerManager.GetPrinterList(out BaseError error);
            if (error != null)
            {
                return BadRequest(error);
            }
            return Ok(new CommonResponse<List<PrinterObject>>(response));

        }
    }
}
