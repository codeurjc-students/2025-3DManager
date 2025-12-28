using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Group;
using _3DMANAGER_APP.Server.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using static _3DMANAGER_APP.Server.Models.Response;

namespace _3DMANAGER_APP.Server.Controllers
{
    [ApiController]
    [Route("api/v1/groups/[action]")]
    public class GroupController : BaseController
    {
        private readonly BLL.Interfaces.IGroupManager _groupManager;

        public GroupController(BLL.Interfaces.IGroupManager groupManager, ILogger<GroupController> logger) : base(logger)
        {
            _groupManager = groupManager;
        }

        /// <summary>
        /// Create a group
        /// </summary>
        /// <returns>A boolean that indicates if the creation has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse<bool>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpPost]
        public CommonResponse<bool> PostNewGroup(GroupRequest request)
        {
            request.UserId = UserId;
            var response = _groupManager.PostNewGroup(request, out BaseError? error);
            if (error != null)
            {
                return new CommonResponse<bool>(new ErrorProperties(error.code, error.message));
            }
            return new CommonResponse<bool>(true);
        }
    }
}
