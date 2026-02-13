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

        const string UnauthorizedMsg = "No autenticado";
        public GroupController(BLL.Interfaces.IGroupManager groupManager, ILogger<GroupController> logger) : base(logger)
        {
            _groupManager = groupManager;
        }

        /// <summary>
        /// Create a group
        /// </summary>
        /// <returns>A boolean that indicates if the creation has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <response code="409">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpPost]
        public IActionResult PostNewGroup(GroupRequest request)
        {
            if (UserId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, UnauthorizedMsg)));

            request.UserId = UserId.Value;
            var response = _groupManager.PostNewGroup(request, out BaseError? error);
            if (error != null)
            {
                return StatusCode(error.code, new Models.CommonResponse<bool>(new ErrorProperties(error.code, error.message)));
            }
            return Ok(new Models.CommonResponse<bool>(response));
        }

        /// <summary>
        /// Get invitations for a group
        /// </summary>
        /// <returns>A list of invitation of groups for a user</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<List<GroupInvitation>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<GroupInvitation>>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<GroupInvitation>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpPost]
        public IActionResult GetGroupInvitations()
        {
            if (UserId == null)
                return Unauthorized(new Models.CommonResponse<List<GroupInvitation>>(new ErrorProperties(401, UnauthorizedMsg)));

            var response = _groupManager.GetGroupInvitations(UserId.Value);

            if (response == null)
                return StatusCode(500, new Models.CommonResponse<List<GroupInvitation>>(new ErrorProperties(500, "Error al obtener la lista de invitaciones a grupo")));

            return Ok(new Models.CommonResponse<List<GroupInvitation>>(response));
        }

        /// <summary>
        /// Accept invitations for a group
        /// </summary>
        /// <returns>True if the operation result was succesfull</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpPost]
        public IActionResult postAcceptInvitation(int groupId, bool isAccepted)
        {
            if (UserId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, UnauthorizedMsg)));

            var response = _groupManager.PostAcceptInvitation(groupId, isAccepted, UserId.Value, out BaseError? error);

            if (error != null)
            {
                return StatusCode(error.code, new Models.CommonResponse<bool>(new ErrorProperties(error.code, error.message)));
            }
            return Ok(new Models.CommonResponse<bool>(response));
        }

        /// <summary>
        /// Update a group
        /// </summary>
        /// <returns>A boolean that indicates if the creation has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<GroupBasicDataResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<GroupBasicDataResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<GroupBasicDataResponse>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpGet]
        public IActionResult GetGroupBasicData()
        {
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<GroupBasicDataResponse>(new ErrorProperties(401, UnauthorizedMsg)));
            var response = _groupManager.GetGroupBasicData(GroupId.Value, out BaseError? error);
            if (error != null)
            {
                return StatusCode(error.code, new Models.CommonResponse<GroupBasicDataResponse>(new ErrorProperties(error.code, error.message)));
            }
            return Ok(new Models.CommonResponse<GroupBasicDataResponse>(response));
        }

        /// <summary>
        /// Update a group
        /// </summary>
        /// <returns>A boolean that indicates if the creation has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpPut]
        public IActionResult UpdateGroupData(GroupRequest request)
        {

            if (UserId == null || GroupId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, UnauthorizedMsg)));

            request.UserId = UserId.Value;
            var response = _groupManager.UpdateGroupData(request, GroupId.Value);
            if (!response)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<bool>(new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al actualizar el grupo")));
            }
            return Ok(new Models.CommonResponse<bool>(true));
        }

        /// <summary>
        /// Fucntion to leave a group
        /// </summary>
        /// <returns>A boolean that indicates if the operation was correct has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpPut]
        public IActionResult UpdateLeaveGroup()
        {
            if (UserId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, UnauthorizedMsg)));

            var response = _groupManager.UpdateLeaveGroup(UserId.Value);

            if (!response)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<bool>(new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al salir del grupo")));
            }
            return Ok(new Models.CommonResponse<bool>(true));
        }

        /// <summary>
        /// Kick an user form the group
        /// </summary>
        /// <returns>A boolean that indicates if the operation was correct has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpPut]
        public Models.CommonResponse<bool> UpdateMembership(int userKickedId)
        {

            var response = _groupManager.UpdateMembership(userKickedId);
            if (!response)
            {
                return new Models.CommonResponse<bool>(new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al expulsar del grupo "));
            }
            return new Models.CommonResponse<bool>(true);
        }

        /// <summary>
        /// Delete a group
        /// </summary>
        /// <returns>A boolean that indicates if the operation was correct has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpDelete]
        public async Task<IActionResult> DeleteGroup()
        {
            if (GroupId == null || UserId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, UnauthorizedMsg)));
            var result = await _groupManager.DeleteGroup(UserId.Value, GroupId.Value);

            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<bool>(new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al eliminar el grupo")));
            }

            return Ok(new Models.CommonResponse<bool>(true));
        }

        /// <summary>
        /// Delete a group
        /// </summary>
        /// <returns>A boolean that indicates if the operation was correct has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpPut]
        public IActionResult TrasnferOwnership(int newOwnerUserId)
        {
            if (GroupId == null || UserId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, UnauthorizedMsg)));

            var result = _groupManager.TrasnferOwnership(UserId.Value, GroupId.Value, newOwnerUserId);

            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<bool>(new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al eliminar el grupo")));
            }

            return Ok(new Models.CommonResponse<bool>(true));
        }

    }
}
