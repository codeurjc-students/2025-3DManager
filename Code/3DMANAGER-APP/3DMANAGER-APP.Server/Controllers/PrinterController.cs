using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Printer;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using static _3DMANAGER_APP.Server.Models.Response;

namespace _3DMANAGER_APP.Server.Controllers
{
    [ApiController]
    [Route("api/v1/printers/[action]")]
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
        [ProducesResponseType(typeof(Models.CommonResponse<List<PrinterObject>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<PrinterObject>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Printers")]
        [HttpGet]
        public IActionResult GetPrinterList()
        {
            _logger.LogInformation("Calling GetPrinterList function");
            List<PrinterObject> response = _printerManager.GetPrinterList(out BaseError error);
            if (error != null)
            {
                return BadRequest(error);
            }
            return Ok(new Models.CommonResponse<List<PrinterObject>>(response));

        }

        /// <summary>
        /// Post a user list
        /// </summary>
        /// <returns>bool</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ApiVersionNeutral]
        [Tags("Printers")]
        [HttpPost]
        public async Task<Models.CommonResponse<bool>> PostPrinter([FromForm] PrinterRequest printer)
        {
            printer.GroupId = GroupId;
            BLL.Models.Base.CommonResponse<bool> response = await _printerManager.PostPrinter(printer);
            if (response.Error != null)
            {
                return new Models.CommonResponse<bool>(new ErrorProperties(response.Error.Code, response.Error.Message));
            }
            return new Models.CommonResponse<bool>(true);
        }

        /// <summary>
        /// Return a filament list
        /// </summary>
        /// <returns>A list of filaments for show in the dasboard</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<List<PrinterListObject>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<PrinterListObject>>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<PrinterListObject>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Printers")]
        [HttpGet]
        public Models.CommonResponse<List<PrinterListObject>> GetPrinterDashboardList()
        {
            List<PrinterListObject> printerList = _printerManager.GetPrinterDashboardList(GroupId, out BaseError error);

            if (printerList == null || error != null)
                return new Models.CommonResponse<List<PrinterListObject>>(new ErrorProperties(error.code, error.message));

            return new Models.CommonResponse<List<PrinterListObject>>(printerList);
        }
    }
}
