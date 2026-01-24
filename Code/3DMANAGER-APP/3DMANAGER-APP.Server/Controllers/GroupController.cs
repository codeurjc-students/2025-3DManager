using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Group;
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
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpPost]
        public Models.CommonResponse<bool> PostNewGroup(GroupRequest request)
        {
            request.UserId = UserId;
            var response = _groupManager.PostNewGroup(request, out BaseError? error);
            if (error != null)
            {
                return new Models.CommonResponse<bool>(new ErrorProperties(error.code, error.message));
            }
            return new Models.CommonResponse<bool>(true);
        }

        /// <summary>
        /// Get invitations for a group
        /// </summary>
        /// <returns>A list of invitation of groups for a user</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<List<GroupInvitation>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<GroupInvitation>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpPost]
        public Models.CommonResponse<List<GroupInvitation>> GetGroupInvitations()
        {
            var response = _groupManager.GetGroupInvitations(UserId);
            return new Models.CommonResponse<List<GroupInvitation>>(response);
        }

        /// <summary>
        /// Accept invitations for a group
        /// </summary>
        /// <returns>True if the operation result was succesfull</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpPost]
        public Models.CommonResponse<bool> postAcceptInvitation(int groupId, bool isAccepted)
        {
            var response = _groupManager.PostAcceptInvitation(groupId, isAccepted, UserId, out BaseError? error);

            if (error != null)
            {
                return new Models.CommonResponse<bool>(new ErrorProperties(error.code, error.message));
            }
            return new Models.CommonResponse<bool>(true);
        }
    }
}
