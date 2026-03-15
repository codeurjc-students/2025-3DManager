using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Group;
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
        private const string NoAuthConstant = "No autenticado";
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
            _logger.LogInformation($"Llamada a la funcion GetPrinterList en el controlador PrinterController");
            List<PrinterObject> response = _printerManager.GetPrinterList(out BaseError? error);
            if (error != null)
            {
                return BadRequest(error);
            }
            return Ok(new Models.CommonResponse<List<PrinterObject>>(response));

        }

        /// <summary>
        /// Post a printer
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
            _logger.LogInformation($"Llamada a la funcion PostPrinter en el controlador PrinterController");
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<int>(new ErrorProperties(401, NoAuthConstant)));

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
        /// Return a printer list
        /// </summary>
        /// <returns>A list of printers for show in the dasboard</returns>
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
            _logger.LogInformation($"Llamada a la funcion GetPrinterDashboardList en el controlador PrinterController");
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<PrinterListObject>(new ErrorProperties(401, NoAuthConstant)));

            List<PrinterListObject> printerList = _printerManager.GetPrinterDashboardList(GroupId.Value, out BaseError? error);

            if (printerList == null || error != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<List<PrinterListObject>>(new ErrorProperties(
                    error == null ? StatusCodes.Status500InternalServerError : error.code,
                    error == null ? $"Error al obtener la lista de impresoras para el dashboard para el grupo {GroupId}" : error.message
                )));
            }
            return Ok(new Models.CommonResponse<List<PrinterListObject>>(printerList));
        }

        /// <summary>
        /// update a printer
        /// </summary>
        /// <returns>Return  a bool taht indicates the success of the operation made</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Printers")]
        [HttpPut]
        public IActionResult UpdatePrinter([FromBody] PrinterDetailRequest request)
        {
            _logger.LogInformation($"Llamada a la funcion UpdatePrinter en el controlador PrinterController");
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, NoAuthConstant)));

            request.GroupId = GroupId.Value;
            bool response = _printerManager.UpdatePrinter(request);

            if (!response)
                return StatusCode(500, new Models.CommonResponse<bool>(new ErrorProperties(StatusCodes.Status500InternalServerError, "Error actualizando la impresora")));

            return Ok(new Models.CommonResponse<bool>(response));
        }


        /// <summary>
        /// Get printer detail
        /// </summary>
        /// <returns>A detail object of a printer</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<PrinterDetailObject>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<PrinterDetailObject>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<PrinterDetailObject>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Printers")]
        [HttpGet]
        public IActionResult GetPrinterDetail([FromQuery] int printerId)
        {
            _logger.LogInformation($"Llamada a la funcion GetPrinterDetail en el controlador PrinterController");
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<PrinterDetailObject>(new ErrorProperties(401, NoAuthConstant)));

            PrinterDetailObject printerResponse = _printerManager.GetPrinterDetail(GroupId.Value, printerId, out BaseError? error);

            if (printerResponse == null || error != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<PrinterDetailObject>(new ErrorProperties(
                error == null ? StatusCodes.Status500InternalServerError : error.code,
                error == null ? "Error al obtener el detalle de impresión" : error.message
            )));
            }

            return Ok(new Models.CommonResponse<PrinterDetailObject>(printerResponse));
        }

        /// <summary>
        /// Delete a printer
        /// </summary>
        /// <returns>Boolean that indicates if operation was succesfull</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <response code="409">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Printers")]
        [HttpDelete]
        public async Task<IActionResult> DeletePrinter([FromQuery] int printerId)
        {
            _logger.LogInformation($"Llamada a la funcion DeletePrinter en el controlador PrinterController");
            if (GroupId == null && UserId == null && UserRole == "Usuario-Manager")
                return Unauthorized(new Models.CommonResponse<GroupBasicDataResponse>(new ErrorProperties(401, NoAuthConstant)));

            BLL.Models.Base.CommonResponse<bool> response = await _printerManager.DeletePrinter(printerId, GroupId!.Value);
            if (response.Error != null || !response.Data)
                return StatusCode(500, new Models.CommonResponse<bool>(new ErrorProperties(response.Error?.Code ?? StatusCodes.Status500InternalServerError, response.Error?.Message ?? "Error al eliminar la impresora")));

            return Ok(new Models.CommonResponse<bool>(response.Data));
        }
    }
}
