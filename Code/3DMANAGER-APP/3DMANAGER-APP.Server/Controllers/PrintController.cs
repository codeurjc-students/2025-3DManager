using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Print;
using _3DMANAGER_APP.Server.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using static _3DMANAGER_APP.Server.Models.Response;

namespace _3DMANAGER_APP.Server.Controllers
{
    [ApiController]
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
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommonResponse<List<PrintListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse<List<PrintListResponse>>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(CommonResponse<List<PrintListResponse>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Print")]
        [HttpGet]
        public CommonResponse<List<PrintListResponse>> GetPrintList([FromQuery] int groupId)
        {
            List<PrintListResponse> userList = _printManager.GetPrintList(groupId, out BaseError error);

            if (userList == null || error != null)
                return new CommonResponse<List<PrintListResponse>>(new ErrorProperties(error.code, error.message));

            return new CommonResponse<List<PrintListResponse>>(userList);
        }
    }
}
