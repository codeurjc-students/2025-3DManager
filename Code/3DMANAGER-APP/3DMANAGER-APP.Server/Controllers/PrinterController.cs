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
        /// <returns>Id of object created</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <response code="409">Conflicto al crear el usuario</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Printers")]
        [HttpPost]
        public async Task<IActionResult> PostPrinter([FromForm] PrinterRequest printer)
        {

            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<int>(new ErrorProperties(401, "No autenticado")));

            printer.GroupId = GroupId.Value;

            BLL.Models.Base.CommonResponse<int> response = await _printerManager.PostPrinter(printer);
            if (response.Error != null)
            {
                if (response.Error.Code == StatusCodes.Status409Conflict)
                    return Conflict(new Models.CommonResponse<int>(new ErrorProperties(response.Error.Code, response.Error.Message)));
                else
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new Models.CommonResponse<int>(new ErrorProperties(response.Error.Code, response.Error.Message)));
            }
            return Ok(new Models.CommonResponse<int>(response.Data));
        }

        /// <summary>
        /// Return a filament list
        /// </summary>
        /// <returns>A list of filaments for show in the dasboard</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<List<PrinterListObject>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<PrinterListObject>>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<PrinterListObject>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Printers")]
        [HttpGet]
        public IActionResult GetPrinterDashboardList()
        {
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<PrinterListObject>(new ErrorProperties(401, "No autenticado")));

            List<PrinterListObject> printerList = _printerManager.GetPrinterDashboardList(GroupId.Value, out BaseError error);

            if (printerList == null || error != null)
                return StatusCode(500, new Models.CommonResponse<List<PrinterListObject>>(new ErrorProperties(error.code, error.message)));

            return Ok(new Models.CommonResponse<List<PrinterListObject>>(printerList));
        }

        /// <summary>
        /// Return a filament list
        /// </summary>
        /// <returns>A list of filaments for show in the dasboard</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<List<PrinterListObject>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<PrinterListObject>>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<PrinterListObject>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Printers")]
        [HttpPut]
        public IActionResult UpdatePrinterState([FromQuery] int printerId, [FromQuery] int stateId)
        {
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<PrinterListObject>(new ErrorProperties(401, "No autenticado")));

            bool response = _printerManager.UpdatePrinterState(GroupId.Value, printerId, stateId, out BaseError error);

            if (error != null)
                return StatusCode(500, new Models.CommonResponse<bool>(new ErrorProperties(error.code, error.message)));

            return Ok(new Models.CommonResponse<bool>(response));
        }
    }
}
