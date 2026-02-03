using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
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
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<PrintListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<PrintListResponse>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Models.CommonResponse<PrintListResponse>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Prints")]
        [HttpGet]
        public Models.CommonResponse<PrintListResponse> GetPrintList([FromQuery] PagedRequest pagination)
        {
            PrintListResponse printList = _printManager.GetPrintList(GroupId, pagination, out BaseError error);

            if (printList == null || error != null)
                return new Models.CommonResponse<PrintListResponse>(new ErrorProperties(error.code, error.message));

            return new Models.CommonResponse<PrintListResponse>(printList);
        }

        /// <summary>
        /// Post a print 3D 
        /// </summary>
        /// <returns>bool</returns>
        /// <response code="200">Respuesta correcta</response>
        /// /// <response code="409">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Prints")]
        [HttpPost]
        public async Task<Models.CommonResponse<int>> PostPrint([FromForm] PrintRequest print)
        {
            print.GroupId = GroupId;
            print.UserId = UserId;
            BLL.Models.Base.CommonResponse<int> response = await _printManager.PostPrint(print);
            if (response.Error != null)
            {
                return new Models.CommonResponse<int>(new ErrorProperties(response.Error.Code, response.Error.Message));
            }
            return new Models.CommonResponse<int>(response.Data);
        }
    }
}
