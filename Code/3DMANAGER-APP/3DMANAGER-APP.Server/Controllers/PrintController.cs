using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.User;
using _3DMANAGER_APP.Server.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using static _3DMANAGER_APP.Server.Models.Response;

namespace _3DMANAGER_APP.Server.Controllers
{
    [ApiController]
    public class PrintController : BaseController
    {
        private readonly IUserManager _userManager;

        public PrintController(IUserManager userManager, ILogger<UserController> logger) : base(logger)
        {
            _userManager = userManager;
        }


        /// <summary>
        /// Return a user list
        /// </summary>
        /// <returns>A list of basic data users for show in the dasboard user list</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommonResponse<List<UserListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse<List<UserListResponse>>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(CommonResponse<List<UserListResponse>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("User")]
        [HttpGet]
        public CommonResponse<List<UserListResponse>> GetUserList([FromQuery] int groupId, out BaseError error)
        {
            List<UserListResponse> userList = _userManager.GetUserList(groupId, out error);

            if (userList == null || error != null)
                return new CommonResponse<List<UserListResponse>>(new ErrorProperties(error.code, error.message));

            return new CommonResponse<List<UserListResponse>>(userList);
        }
    }
}
