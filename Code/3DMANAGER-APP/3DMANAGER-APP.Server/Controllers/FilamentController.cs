using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Filament;
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
        [ProducesResponseType(typeof(Models.CommonResponse<List<FilamentListResponse>>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<FilamentListResponse>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Filaments")]
        [Authorize]
        [HttpGet]
        public Models.CommonResponse<List<FilamentListResponse>> GetFilamentList()
        {
            List<FilamentListResponse> userList = _filamentManager.GetFilamentList(GroupId, out BaseError error);

            if (userList == null || error != null)
                return new Models.CommonResponse<List<FilamentListResponse>>(new ErrorProperties(error.code, error.message));

            return new Models.CommonResponse<List<FilamentListResponse>>(userList);
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
        [Tags("Filaments")]
        [HttpPost]
        public Models.CommonResponse<bool> PostFilament(FilamentRequest filament)
        {
            filament.GroupId = GroupId;
            _filamentManager.PostFilament(filament, out BaseError? error);
            if (error != null)
            {
                return new Models.CommonResponse<bool>(new ErrorProperties(error.code, error.message));
            }
            return new Models.CommonResponse<bool>(true);
        }
    }
}
