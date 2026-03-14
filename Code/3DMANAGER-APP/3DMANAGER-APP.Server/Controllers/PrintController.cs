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
        private const string NoAuthConstant = "No autenticado";
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
            _logger.LogInformation($"Llamada a la funcion GetPrintList en el controlador PrintController");
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<PrintListResponse>(new ErrorProperties(401, NoAuthConstant)));

            PrintListResponse printList = _printManager.GetPrintList(GroupId.Value, pagination, out BaseError? error);

            if (printList == null || error != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<PrintListResponse>(new ErrorProperties(
                error == null ? StatusCodes.Status500InternalServerError : error.code,
                error == null ? "Error al obtener la lista" : error.message
            )));
            }



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
            _logger.LogInformation($"Llamada a la funcion PostPrint en el controlador PrintController");
            if (GroupId == null || UserId == null)
                return Unauthorized(new Models.CommonResponse<GroupBasicDataResponse>(new ErrorProperties(401, NoAuthConstant)));

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
            _logger.LogInformation($"Llamada a la funcion GetPrintListByType en el controlador PrintController");
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<PrintListResponse>(new ErrorProperties(401, NoAuthConstant)));

            PrintListResponse printList = _printManager.GetPrintListByType(GroupId.Value, pagination, type, id, out BaseError? error);

            if (printList == null || error != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<PrintListResponse>(new ErrorProperties(
                error == null ? StatusCodes.Status500InternalServerError : error.code,
                error == null ? "Error al obtener la lista segun el tipo" : error.message
            )));
            }

            return Ok(new Models.CommonResponse<PrintListResponse>(printList));
        }

        /// <summary>
        /// update a print
        /// </summary>
        /// <returns>Return  a bool that indicates the success of the operation made</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Prints")]
        [HttpPut]
        public IActionResult UpdatePrint([FromBody] PrintDetailRequest request)
        {
            _logger.LogInformation($"Llamada a la funcion UpdatePrint en el controlador PrintController");
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, NoAuthConstant)));

            request.GroupId = GroupId.Value;
            bool response = _printManager.UpdatePrint(request);

            if (!response)
                return StatusCode(500, new Models.CommonResponse<bool>(new ErrorProperties(StatusCodes.Status500InternalServerError, "Error actualizando la impresión")));

            return Ok(new Models.CommonResponse<bool>(response));
        }


        /// <summary>
        /// Get print detail
        /// </summary>
        /// <returns>A detail object of a print</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<PrintDetailObject>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<PrintDetailObject>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<PrintDetailObject>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Prints")]
        [HttpGet]
        public IActionResult GetPrintDetail([FromQuery] int printId)
        {
            _logger.LogInformation($"Llamada a la funcion GetPrintDetail en el controlador PrintController");
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<PrintDetailObject>(new ErrorProperties(401, NoAuthConstant)));

            PrintDetailObject printerResponse = _printManager.GetPrintDetail(GroupId.Value, printId, out BaseError? error);

            if (printerResponse == null || error != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<PrintDetailObject>(new ErrorProperties(
                error == null ? StatusCodes.Status500InternalServerError : error.code,
                error == null ? "Error al obtener el detalle de impresión" : error.message
            )));
            }

            return Ok(new Models.CommonResponse<PrintDetailObject>(printerResponse));
        }

        /// <summary>
        /// Get print comments
        /// </summary>
        /// <returns>A list object of a print comments</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<List<PrintCommentObject>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<PrintCommentObject>>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<PrintCommentObject>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Prints")]
        [HttpGet]
        public IActionResult GetPrintComments([FromQuery] int printId)
        {
            _logger.LogInformation($"Llamada a la funcion GetPrintComments en el controlador PrintController");
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<List<PrintCommentObject>>(new ErrorProperties(401, NoAuthConstant)));

            var comments = _printManager.GetPrintComments(GroupId.Value, printId, out BaseError? error);

            if (comments == null || error != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<List<PrintCommentObject>>(new ErrorProperties(
                error == null ? StatusCodes.Status500InternalServerError : error.code,
                error == null ? $"Error al obtener los comentarios de la impresión {printId}" : error.message
            )));
            }

            return Ok(new CommonResponse<List<PrintCommentObject>>(comments));
        }

        /// <summary>
        /// Post comment on a print
        /// </summary>
        /// <returns>Id of the print comment created</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Prints")]
        [HttpPost]
        public IActionResult PostPrintComment([FromBody] PrintCommentRequest request)
        {
            _logger.LogInformation($"Llamada a la funcion PostPrintComment en el controlador PrintController");
            if (GroupId == null && UserId == null)
                return Unauthorized(new Models.CommonResponse<int>(new ErrorProperties(401, NoAuthConstant)));

            request.UserId = UserId!.Value;

            int newId = _printManager.PostPrintComment(request, out BaseError? error);

            if (error != null || newId == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<int>(new ErrorProperties(
                error == null ? StatusCodes.Status500InternalServerError : error.code,
                error == null ? "Error al obtener la lista" : error.message
            )));
            }

            return Ok(new CommonResponse<int>(newId));
        }

        /// <summary>
        /// Delete a print 3D 
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
        [Tags("Prints")]
        [HttpDelete]
        public async Task<IActionResult> DeletePrint([FromQuery] int printId)
        {
            _logger.LogInformation($"Llamada a la funcion DeletePrint en el controlador PrintController");
            if (GroupId == null && UserId == null && UserRole == "Usuario-Manager")
                return Unauthorized(new Models.CommonResponse<GroupBasicDataResponse>(new ErrorProperties(401, NoAuthConstant)));

            BLL.Models.Base.CommonResponse<bool> response = await _printManager.DeletePrint(printId, GroupId!.Value);
            if (response.Error != null || !response.Data)
                return StatusCode(500, new Models.CommonResponse<bool>(new ErrorProperties(response.Error?.Code ?? StatusCodes.Status500InternalServerError, response.Error?.Message ?? "Error al eliminar la impresión")));

            return Ok(new Models.CommonResponse<bool>(response.Data));
        }

    }
}
