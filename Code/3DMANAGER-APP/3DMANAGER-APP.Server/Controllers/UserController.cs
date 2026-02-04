using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.User;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using static _3DMANAGER_APP.Server.Models.Response;

namespace _3DMANAGER_APP.Server.Controllers
{
    [ApiController]
    [Route("api/v1/users/[action]")]
    public class UserController : BaseController
    {
        private readonly IUserManager _userManager;
        private readonly JwtService _jwtService;

        public UserController(IUserManager userManager, ILogger<UserController> logger, JwtService jwtService) : base(logger)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Create a user 
        /// </summary>
        /// <returns>A boolean that indicates if the creation has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Models.CommonResponse<int>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpPost]
        public async Task<Models.CommonResponse<int>> PostNewUser([FromForm] UserCreateRequest user)
        {
            BLL.Models.Base.CommonResponse<int> response = await _userManager.PostNewUser(user);
            if (response.Error != null)
            {
                return new Models.CommonResponse<int>(new ErrorProperties(response.Error.Code, response.Error.Message));
            }
            return new Models.CommonResponse<int>(response.Data);
        }


        /// <summary>
        /// Log as  user 
        /// </summary>
        /// <returns>A boolean that indicates if the creation has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<LoginResponse>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Models.CommonResponse<LoginResponse>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpPost]
        public Models.CommonResponse<LoginResponse> Login(BLL.Models.User.LoginRequest request)
        {
            UserObject user = _userManager.Login(request.UserName, request.UserPassword, out BaseError error);

            if (user == null || error != null)
                return new Models.CommonResponse<LoginResponse>(new ErrorProperties(error.code, error.message));

            var token = _jwtService.GenerateToken(user);

            LoginResponse response = new LoginResponse();
            response.User = user;
            response.Token = token;

            return new Models.CommonResponse<LoginResponse>(response);
        }

        /// <summary>
        /// Log as guest user 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<LoginResponse>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Models.CommonResponse<LoginResponse>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpPost]
        public Models.CommonResponse<LoginResponse> LoginGuest()
        {
            string userName = "Invitado";
            string userPassword = "invitado3dmanager";
            UserObject user = _userManager.Login(userName, userPassword, out BaseError error);

            if (user == null || error != null)
                return new Models.CommonResponse<LoginResponse>(new ErrorProperties(error.code, error.message));

            var token = _jwtService.GenerateToken(user);

            LoginResponse response = new LoginResponse();
            response.User = user;
            response.Token = token;

            return new Models.CommonResponse<LoginResponse>(response);
        }

        /// <summary>
        /// Return a user list
        /// </summary>
        /// <returns>A list of basic data users for show in the dasboard user list</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<List<UserListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<UserListResponse>>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<UserListResponse>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpGet]
        public Models.CommonResponse<List<UserListResponse>> GetUserList()
        {
            List<UserListResponse> userList = _userManager.GetUserList(GroupId, out BaseError error);

            if (userList == null || error != null)
                return new Models.CommonResponse<List<UserListResponse>>(new ErrorProperties(error.code, error.message));

            return new Models.CommonResponse<List<UserListResponse>>(userList);
        }

        /// <summary>
        /// Return a user list
        /// </summary>
        /// <returns>A list of basic data users to invite for show in the dasboard user list invitation</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<List<UserListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<UserListResponse>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpGet]
        public Models.CommonResponse<List<UserListResponse>> GetUserInvitationList([FromQuery] string? filter)
        {
            List<UserListResponse> userList = _userManager.GetUserInvitationList(filter, out BaseError error);

            if (userList == null || error != null)
                return new Models.CommonResponse<List<UserListResponse>>(new ErrorProperties(error.code, error.message));

            return new Models.CommonResponse<List<UserListResponse>>(userList);
        }

        /// <summary>
        /// Invites a user to a group
        /// </summary>
        /// <returns>Return a bool that verifies if the invitatios was succesfull or not</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpPost]
        public Models.CommonResponse<bool> PostUserInvitation([FromQuery] int userId)
        {
            var groupId = GroupId;
            bool response = _userManager.PostUserInvitation(groupId, userId, out BaseError? error);
            if (error != null)
                return new Models.CommonResponse<bool>(new ErrorProperties(error.code, error.message));
            else
                return new Models.CommonResponse<bool>(response);
        }
    }
}
