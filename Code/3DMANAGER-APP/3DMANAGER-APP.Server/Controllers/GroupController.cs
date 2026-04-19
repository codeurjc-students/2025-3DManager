using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Group;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static _3DMANAGER_APP.Server.Models.Response;

namespace _3DMANAGER_APP.Server.Controllers
{
    [ApiController]
    [Route("api/v1/groups/[action]")]
    public class GroupController : BaseController
    {
        private readonly BLL.Interfaces.IGroupService _groupService;

        const string UnauthorizedMsg = "No autenticado";
        public GroupController(BLL.Interfaces.IGroupService groupService, ILogger<GroupController> logger) : base(logger)
        {
            _groupService = groupService;
        }

        /// <summary>
        /// Create a group
        /// </summary>
        /// <returns>A boolean that indicates if the creation has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <response code="409">Conflicto</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Authorize(Roles = "Usuario-Base")]
        [Tags("Groups")]
        [HttpPost]
        public IActionResult PostNewGroup(GroupRequest request)
        {
            _logger.LogInformation($"Llamada a la funcion PostNewGroup en el controlador GroupController");
            if (UserId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, UnauthorizedMsg)));

            request.UserId = UserId.Value;
            var response = _groupService.PostNewGroup(request, out BaseError? error);
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
        [Authorize]
        [Tags("Groups")]
        [HttpPost]
        public IActionResult GetGroupInvitations()
        {
            _logger.LogInformation($"Llamada a la funcion GetGroupInvitations en el controlador GroupController");
            if (UserId == null)
                return Unauthorized(new Models.CommonResponse<List<GroupInvitation>>(new ErrorProperties(401, UnauthorizedMsg)));

            var response = _groupService.GetGroupInvitations(UserId.Value, out bool error);

            if (error)
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
        [Authorize(Roles = "Usuario-Base")]
        [Tags("Groups")]
        [HttpPost]
        public IActionResult PostAcceptInvitation(int groupId, bool isAccepted)
        {
            _logger.LogInformation($"Llamada a la funcion postAcceptInvitation en el controlador GroupController");
            if (UserId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, UnauthorizedMsg)));

            var response = _groupService.PostAcceptInvitation(groupId, isAccepted, UserId.Value, out BaseError? error);

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
        [Authorize]
        [Tags("Groups")]
        [HttpGet]
        public IActionResult GetGroupBasicData()
        {
            _logger.LogInformation($"Llamada a la funcion GetGroupBasicData en el controlador GroupController");
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<GroupBasicDataResponse>(new ErrorProperties(401, UnauthorizedMsg)));
            var response = _groupService.GetGroupBasicData(GroupId.Value, out BaseError? error);
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
        [Authorize(Roles = "Usuario-Manager")]
        [Tags("Groups")]
        [HttpPut]
        public IActionResult UpdateGroupData(GroupRequest request)
        {
            _logger.LogInformation($"Llamada a la funcion UpdateGroupData en el controlador GroupController");
            if (UserId == null || GroupId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, UnauthorizedMsg)));

            request.UserId = UserId.Value;
            var response = _groupService.UpdateGroupData(request, GroupId.Value);
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
        [Authorize(Roles = "Usuario-Base,Usuario-Manager")]
        [Tags("Groups")]
        [HttpPut]
        public IActionResult UpdateLeaveGroup()
        {
            _logger.LogInformation($"Llamada a la funcion UpdateLeaveGroup en el controlador GroupController");
            if (UserId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, UnauthorizedMsg)));

            var response = _groupService.UpdateLeaveGroup(UserId.Value);

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
        [Authorize(Roles = "Usuario-Manager")]
        [Tags("Groups")]
        [HttpPut]
        public IActionResult UpdateMembership(int userKickedId)
        {
            if (UserId == null || UserRole != "Usuario-Manager")
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, UnauthorizedMsg)));
            _logger.LogInformation($"Llamada a la funcion UpdateMembership en el controlador GroupController");
            var response = _groupService.UpdateMembership(userKickedId, UserId.Value);
            if (!response)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<bool>(new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al expulsar del grupo ")));
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
        [Authorize(Roles = "Usuario-Manager")]
        [Tags("Groups")]
        [HttpDelete]
        public async Task<IActionResult> DeleteGroup()
        {
            _logger.LogInformation($"Llamada a la funcion DeleteGroup en el controlador GroupController");
            if (GroupId == null || UserId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, UnauthorizedMsg)));
            var result = await _groupService.DeleteGroup(UserId.Value, GroupId.Value);

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
        [Authorize(Roles = "Usuario-Manager")]
        [Tags("Groups")]
        [HttpPut]
        public IActionResult TrasnferOwnership(int newOwnerUserId)
        {
            _logger.LogInformation($"Llamada a la funcion TrasnferOwnership en el controlador GroupController");
            if (GroupId == null || UserId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, UnauthorizedMsg)));

            var result = _groupService.TrasnferOwnership(UserId.Value, GroupId.Value, newOwnerUserId);

            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<bool>(new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al eliminar el grupo")));
            }

            return Ok(new Models.CommonResponse<bool>(true));
        }

        /// <summary>
        /// Get group data info for dashboard
        /// </summary>
        /// <returns>A boolean that indicates if the creation has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<GroupDashboardData>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<GroupDashboardData>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<GroupDashboardData>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Authorize]
        [Tags("Groups")]
        [HttpGet]
        public IActionResult GetGroupDashboardData()
        {
            _logger.LogInformation($"Llamada a la funcion GetGroupDashboardData en el controlador GroupController");
            if (GroupId == null || GroupId == 0)
                return Unauthorized(new Models.CommonResponse<GroupDashboardData>(new ErrorProperties(401, UnauthorizedMsg)));
            var response = _groupService.GetGroupDashboardData(GroupId.Value, out BaseError? error);
            if (error != null)
            {
                return StatusCode(error.code, new Models.CommonResponse<GroupDashboardData>(new ErrorProperties(error.code, error.message)));
            }
            return Ok(new Models.CommonResponse<GroupDashboardData>(response));
        }

    }
}
