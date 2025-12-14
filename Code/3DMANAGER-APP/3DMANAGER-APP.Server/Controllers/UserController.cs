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
        [ProducesResponseType(typeof(CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse<bool>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpPost]
        public CommonResponse<bool> PostNewUser(UserCreateRequest user)
        {
            var response = _userManager.PostNewUser(user, out BaseError? error);
            if (error != null)
            {
                return new CommonResponse<bool>(new ErrorProperties(error.code, error.message));
            }
            return new CommonResponse<bool>(true);
        }


        /// <summary>
        /// Log as  user 
        /// </summary>
        /// <returns>A boolean that indicates if the creation has been successful</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommonResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse<LoginResponse>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(CommonResponse<LoginResponse>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpPost]
        public CommonResponse<LoginResponse> Login(BLL.Models.User.LoginRequest request)
        {
            UserObject user = _userManager.Login(request.UserName, request.UserPassword, out BaseError error);

            if (user == null || error != null)
                return new CommonResponse<LoginResponse>(new ErrorProperties(error.code, error.message));

            var token = _jwtService.GenerateToken(user);

            LoginResponse response = new LoginResponse();
            response.User = user;
            response.Token = token;

            return new CommonResponse<LoginResponse>(response);
        }

        /// <summary>
        /// Log as guest user 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommonResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse<LoginResponse>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(CommonResponse<LoginResponse>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpPost]
        public CommonResponse<LoginResponse> LoginGuest()
        {
            string userName = "Invitado";
            string userPassword = "invitado3dmanager";
            UserObject user = _userManager.Login(userName, userPassword, out BaseError error);

            if (user == null || error != null)
                return new CommonResponse<LoginResponse>(new ErrorProperties(error.code, error.message));

            var token = _jwtService.GenerateToken(user);

            LoginResponse response = new LoginResponse();
            response.User = user;
            response.Token = token;

            return new CommonResponse<LoginResponse>(response);
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
        [Tags("Users")]
        [HttpGet]
        public CommonResponse<List<UserListResponse>> GetUserList([FromQuery] int groupId)
        {
            List<UserListResponse> userList = _userManager.GetUserList(groupId, out BaseError error);

            if (userList == null || error != null)
                return new CommonResponse<List<UserListResponse>>(new ErrorProperties(error.code, error.message));

            return new CommonResponse<List<UserListResponse>>(userList);
        }
    }
}
