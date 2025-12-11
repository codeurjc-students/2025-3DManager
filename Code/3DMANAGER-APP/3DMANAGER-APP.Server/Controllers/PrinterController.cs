using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Printer;
using _3DMANAGER_APP.Server.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using static _3DMANAGER_APP.Server.Models.Response;

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

        /// <summary>
        /// Post a user list
        /// </summary>
        /// <returns>bool</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommonResponse<bool>), StatusCodes.Status200OK)]
        [ApiVersionNeutral]
        [Tags("Printer")]
        [HttpPost]
        public CommonResponse<bool> PostPrinter(PrinterRequest printer)
        {
            _printerManager.PostPrinter(printer, out BaseError? error);
            if (error != null)
            {
                return new CommonResponse<bool>(new ErrorProperties(error.code, error.message));
            }
            return new CommonResponse<bool>(true);
        }

        /// <summary>
        /// Return a filament list
        /// </summary>
        /// <returns>A list of filaments for show in the dasboard</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommonResponse<List<PrinterListObject>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse<List<PrinterListObject>>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(CommonResponse<List<PrinterListObject>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Filament")]
        [HttpGet]
        public CommonResponse<List<PrinterListObject>> GetPrinterDashboardList([FromQuery] int groupId)
        {
            List<PrinterListObject> printerList = _printerManager.GetPrinterDashboardList(groupId, out BaseError error);

            if (printerList == null || error != null)
                return new CommonResponse<List<PrinterListObject>>(new ErrorProperties(error.code, error.message));

            return new CommonResponse<List<PrinterListObject>>(printerList);
        }
    }
}
