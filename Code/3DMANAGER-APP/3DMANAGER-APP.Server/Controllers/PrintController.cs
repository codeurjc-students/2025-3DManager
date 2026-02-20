using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Group;
using _3DMANAGER_APP.BLL.Models.Print;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using static _3DMANAGER_APP.Server.Models.Response;

namespace _3DMANAGER_APP.Server.Controllers
{
    [ApiController]
    [Route("api/v1/prints/[action]")]
    public class PrintController : BaseController
    {
        private readonly IPrintManager _printManager;

        public PrintController(IPrintManager printManager, ILogger<PrintController> logger) : base(logger)
        {
            _printManager = printManager;
        }

        /// <summary>
        /// Return a user list
        /// </summary>
        /// <returns>A list of basic data users for show in the dasboard user list</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<PrintListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<PrintListResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<PrintListResponse>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Prints")]
        [HttpGet]
        public IActionResult GetPrintList([FromQuery] PagedRequest pagination)
        {
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<PrintListResponse>(new ErrorProperties(401, "No autenticado")));

            PrintListResponse printList = _printManager.GetPrintList(GroupId.Value, pagination, out BaseError? error);

            if (printList == null || error != null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<PrintListResponse>(new ErrorProperties(error.code, error.message)));

            return Ok(new Models.CommonResponse<PrintListResponse>(printList));
        }

        /// <summary>
        /// Post a print 3D 
        /// </summary>
        /// <returns>Id of object created</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <response code="409">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Prints")]
        [HttpPost]
        public async Task<IActionResult> PostPrint([FromForm] PrintRequest print)
        {
            if (GroupId == null || UserId == null)
                return Unauthorized(new Models.CommonResponse<GroupBasicDataResponse>(new ErrorProperties(401, "No autenticado")));

            print.GroupId = GroupId.Value;
            print.UserId = UserId.Value;
            BLL.Models.Base.CommonResponse<int> response = await _printManager.PostPrint(print);
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
        /// Return a user list of prints printed by a especific printer
        /// </summary>
        /// <returns>A list of basic data users for show in the dasboard user list</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<PrintListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<PrintListResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<PrintListResponse>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Prints")]
        [HttpGet]
        public IActionResult GetPrintListByType([FromQuery] PagedRequest pagination, [FromQuery] int type, [FromQuery] int id)
        {
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<PrintListResponse>(new ErrorProperties(401, "No autenticado")));

            PrintListResponse printList = _printManager.GetPrintListByType(GroupId.Value, pagination, type, id, out BaseError? error);

            if (printList == null || error != null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<PrintListResponse>(new ErrorProperties(error.code, error.message)));

            return Ok(new Models.CommonResponse<PrintListResponse>(printList));
        }
    }
}
