using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Filament;
using _3DMANAGER_APP.Server.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using static _3DMANAGER_APP.Server.Models.Response;

namespace _3DMANAGER_APP.Server.Controllers
{
    [ApiController]
    public class FilamentController : BaseController
    {
        private readonly IFilamentManager _filamentManager;

        public FilamentController(IFilamentManager filamentManager, ILogger<FilamentController> logger) : base(logger)
        {
            _filamentManager = _filamentManager;
        }



        /// <summary>
        /// Return a user list
        /// </summary>
        /// <returns>A list of basic data users for show in the dasboard user list</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommonResponse<List<FilamentListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse<List<FilamentListResponse>>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(CommonResponse<List<FilamentListResponse>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Filament")]
        [HttpGet]
        public CommonResponse<List<FilamentListResponse>> GetFilamentList([FromRoute] int groupId)
        {
            List<FilamentListResponse> userList = _filamentManager.GetFilamentList(groupId, out BaseError error);

            if (userList == null || error != null)
                return new CommonResponse<List<FilamentListResponse>>(new ErrorProperties(error.code, error.message));

            return new CommonResponse<List<FilamentListResponse>>(userList);
        }
    }
}
