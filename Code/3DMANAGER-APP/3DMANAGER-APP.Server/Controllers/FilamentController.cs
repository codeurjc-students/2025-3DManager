using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Filament;
using _3DMANAGER_APP.BLL.Models.Printer;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static _3DMANAGER_APP.Server.Models.Response;

namespace _3DMANAGER_APP.Server.Controllers
{
    [ApiController]
    [Route("api/v1/filaments/[action]")]
    public class FilamentController : BaseController
    {
        private readonly IFilamentManager _filamentManager;

        public FilamentController(IFilamentManager filamentManager, ILogger<FilamentController> logger) : base(logger)
        {
            _filamentManager = filamentManager;
        }



        /// <summary>
        /// Return a filament list
        /// </summary>
        /// <returns>A list of filaments for show in the dasboard</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<List<FilamentListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<FilamentListResponse>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<FilamentListResponse>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Filaments")]
        [Authorize]
        [HttpGet]
        public IActionResult GetFilamentList()
        {
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<List<FilamentListResponse>>(new ErrorProperties(401, "No autenticado")));

            List<FilamentListResponse> userList = _filamentManager.GetFilamentList(GroupId.Value, out BaseError error);

            if (error != null)
            {
                return StatusCode(error.code, new Models.CommonResponse<List<FilamentListResponse>>(new ErrorProperties(error.code, error.message)));
            }

            return Ok(new Models.CommonResponse<List<FilamentListResponse>>(userList));
        }

        /// <summary>
        /// Post a user list
        /// </summary>
        /// <returns>Id of object created</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <response code="409">Conlficto</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Filaments")]
        [HttpPost]
        public async Task<IActionResult> PostFilament([FromForm] FilamentRequest filament)
        {

            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<int>(new ErrorProperties(401, "No autenticado")));

            filament.GroupId = GroupId.Value;

            BLL.Models.Base.CommonResponse<int> response = await _filamentManager.PostFilament(filament);
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
        /// update a filament
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
        public IActionResult UpdateFilament([FromBody] PrinterDetailRequest request)
        {
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, "No autenticado")));

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
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<PrinterDetailObject>(new ErrorProperties(401, "No autenticado")));

            PrinterDetailObject printerResponse = _printerManager.GetPrinterDetail(GroupId.Value, printerId, out BaseError error);

            if (printerResponse == null || error != null)
                return StatusCode(500, new Models.CommonResponse<PrinterDetailObject>(new ErrorProperties(error.code, error.message)));

            return Ok(new Models.CommonResponse<PrinterDetailObject>(printerResponse));
        }
    }
}
