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

        /// <summary>
        /// Update a group
        /// </summary>
        /// <returns>A boolean that indicates if the creation has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<GroupBasicDataResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<GroupBasicDataResponse>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Models.CommonResponse<GroupBasicDataResponse>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpGet]
        public Models.CommonResponse<GroupBasicDataResponse> GetGroupBasicData()
        {
            var response = _groupManager.GetGroupBasicData(GroupId, out BaseError? error);
            if (error != null)
            {
                return new Models.CommonResponse<GroupBasicDataResponse>(new ErrorProperties(error.code, error.message));
            }
            return new Models.CommonResponse<GroupBasicDataResponse>(response);
        }

        /// <summary>
        /// Update a group
        /// </summary>
        /// <returns>A boolean that indicates if the creation has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpPut]
        public Models.CommonResponse<bool> UpdateGroupData(GroupRequest request)
        {
            request.UserId = UserId;
            var response = _groupManager.UpdateGroupData(request, GroupId);
            if (!response)
            {
                return new Models.CommonResponse<bool>(new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al actualizar el grupo"));
            }
            return new Models.CommonResponse<bool>(true);
        }

        /// <summary>
        /// Fucntion to leave a group
        /// </summary>
        /// <returns>A boolean that indicates if the operation was correct has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpPut]
        public Models.CommonResponse<bool> UpdateLeaveGroup()
        {

            var response = _groupManager.UpdateLeaveGroup(UserId);
            if (!response)
            {
                return new Models.CommonResponse<bool>(new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al salir del grupo"));
            }
            return new Models.CommonResponse<bool>(true);
        }

        /// <summary>
        /// Kick an user form the group
        /// </summary>
        /// <returns>A boolean that indicates if the operation was correct has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
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
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpDelete]
        public async Task<Models.CommonResponse<bool>> DeleteGroup()
        {
            var result = await _groupManager.DeleteGroup(UserId, GroupId);

            if (!result)
            {
                return new Models.CommonResponse<bool>(new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al eliminar el grupo"));
            }

            return new Models.CommonResponse<bool>(true);
        }

        /// <summary>
        /// Delete a group
        /// </summary>
        /// <returns>A boolean that indicates if the operation was correct has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Groups")]
        [HttpPut]
        public Models.CommonResponse<bool> TrasnferOwnership(int newOwnerUserId)
        {
            var result = _groupManager.TrasnferOwnership(UserId, GroupId, newOwnerUserId);

            if (!result)
            {
                return new Models.CommonResponse<bool>(new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al eliminar el grupo"));
            }

            return new Models.CommonResponse<bool>(true);
        }

    }
}
