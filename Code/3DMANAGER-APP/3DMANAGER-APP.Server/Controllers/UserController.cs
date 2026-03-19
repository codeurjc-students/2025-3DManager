using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Group;
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
        private const string NoAuthConstant = "No autenticado";
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
            _logger.LogInformation($"Llamada a la funcion PostNewUser en el controlador UserController");
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
        [ProducesResponseType(typeof(Models.CommonResponse<LoginResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<LoginResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Models.CommonResponse<LoginResponse>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpPost]
        public Models.CommonResponse<LoginResponse> Login(BLL.Models.User.LoginRequest request)
        {
            _logger.LogInformation($"Llamada a la funcion Login en el controlador UserController");
            UserObject user = _userManager.Login(request.UserName, request.UserPassword, out BaseError? error);

            if (user == null || error != null)
            {
                return new Models.CommonResponse<LoginResponse>(new ErrorProperties(
                error == null ? StatusCodes.Status500InternalServerError : error.code,
                error == null ? $"Error al hacer el login de usuario {request.UserName}" : error.message
                ));
            }
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
        [ProducesResponseType(typeof(Models.CommonResponse<LoginResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<LoginResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Models.CommonResponse<LoginResponse>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpPost]
        public Models.CommonResponse<LoginResponse> LoginGuest()
        {
            _logger.LogInformation($"Llamada a la funcion LoginGuest en el controlador UserController");
            string userName = "Invitado";
            UserObject user = _userManager.Login(userName, "invitado3dmanager", out BaseError? error);

            if (user == null || error != null)
            {
                return new Models.CommonResponse<LoginResponse>(new ErrorProperties(
                error == null ? StatusCodes.Status500InternalServerError : error.code,
                error == null ? "Error al hacer el login de invitado" : error.message
                ));
            }
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
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<List<UserListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<UserListResponse>>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<UserListResponse>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpGet]
        public IActionResult GetUserList()
        {
            _logger.LogInformation($"Llamada a la funcion GetUserList en el controlador UserController");
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<UserListResponse>(new ErrorProperties(401, NoAuthConstant)));

            List<UserListResponse> userList = _userManager.GetUserList(GroupId.Value, out BaseError? error);

            if (error != null)
                return StatusCode(error.code, new Models.CommonResponse<List<UserListResponse>>(new ErrorProperties(error.code, error.message)));

            return Ok(new Models.CommonResponse<List<UserListResponse>>(userList));
        }

        /// <summary>
        /// Return a user list
        /// </summary>
        /// <returns>A list of basic data users to invite for show in the dasboard user list invitation</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<List<UserListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<UserListResponse>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpGet]
        public Models.CommonResponse<List<UserListResponse>> GetUserInvitationList([FromQuery] string? filter)
        {
            _logger.LogInformation($"Llamada a la funcion GetUserInvitationList en el controlador UserController");
            List<UserListResponse> userList = _userManager.GetUserInvitationList(filter, out BaseError? error);

            if (error != null)
                return new Models.CommonResponse<List<UserListResponse>>(new ErrorProperties(error.code, error.message));

            return new Models.CommonResponse<List<UserListResponse>>(userList);
        }

        /// <summary>
        /// Invites a user to a group
        /// </summary>
        /// <returns>Return a bool that verifies if the invitatios was succesfull or not</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpPost]
        public IActionResult PostUserInvitation([FromQuery] int userId)
        {
            _logger.LogInformation($"Llamada a la funcion PostUserInvitation en el controlador UserController");
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, NoAuthConstant)));
            bool response = _userManager.PostUserInvitation(GroupId.Value, userId, out BaseError? error);
            if (error != null)
                return StatusCode(error.code, new Models.CommonResponse<bool>(new ErrorProperties(error.code, error.message)));
            else
                return Ok(new Models.CommonResponse<bool>(response));
        }

        /// <summary>
        /// Invites a user to a group
        /// </summary>
        /// <returns>Return a bool that verifies if the invitatios was succesfull or not</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status401Unauthorized)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpGet]
        public IActionResult GetUserAuth()
        {
            _logger.LogInformation($"Llamada a la funcion GetUserAuth en el controlador UserController");
            if (UserId == null)
                return Unauthorized();

            var user = _userManager.GetUserById(UserId.Value);

            if (user == null || user.UserId == 0)
                return Unauthorized();

            return Ok(new
            {
                userId = user.UserId,
                groupId = user.GroupId,
                rolId = user.RolId,
                groupName = user.GroupName
            });
        }

        /// <summary>
        /// update the user
        /// </summary>
        /// <returns>Return  a bool taht indicates the success of the operation made</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpPut]
        public IActionResult UpdateUser([FromBody] UserUpdateRequest request)
        {
            _logger.LogInformation($"Llamada a la funcion UpdateUser en el controlador UserController");
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, NoAuthConstant)));

            request.GroupId = GroupId.Value;
            bool response = _userManager.UpdateUser(request);

            if (!response)
                return StatusCode(500, new Models.CommonResponse<bool>(new ErrorProperties(StatusCodes.Status500InternalServerError, "Error actualizando el usuario")));

            return Ok(new Models.CommonResponse<bool>(response));
        }


        /// <summary>
        /// Return a user detail
        /// </summary>
        /// <returns>A list of filaments for show in the dasboard</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<UserDetailObject>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<UserDetailObject>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<UserDetailObject>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpGet]
        public IActionResult GetUserDetail([FromQuery] int userId)
        {
            _logger.LogInformation($"Llamada a la funcion GetUserDetail en el controlador UserController");
            if (GroupId == null)
                return Unauthorized(new Models.CommonResponse<UserDetailObject>(new ErrorProperties(401, NoAuthConstant)));

            UserDetailObject userResponse = _userManager.GetUserDetail(GroupId.Value, userId, out BaseError? error);

            if (userResponse == null || error != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<UserDetailObject>(new ErrorProperties(
                error == null ? StatusCodes.Status500InternalServerError : error.code,
                error == null ? $"Error al obtener el detalle de usuario {userId}" : error.message
            )));
            }

            return Ok(new Models.CommonResponse<UserDetailObject>(userResponse));
        }

        /// <summary>
        /// Update a user image
        /// </summary>
        /// <returns>Boolean that indicates if operation was succesfull</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <response code="409">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserImage(int userId, IFormFile imageFile)
        {
            _logger.LogInformation($"Llamada a UpdateUserImage en el controlador UserController");
            if (GroupId == null && UserId == null && UserRole == "Usuario-Manager")
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, NoAuthConstant)));

            var result = await _userManager.UpdateUserImage(userId, GroupId!.Value, imageFile);

            if (result.Error != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new CommonResponse<bool>(result.Error));

            return Ok(new CommonResponse<bool>(true));
        }

        /// <summary>
        /// Delete a user image
        /// </summary>
        /// <returns>Boolean that indicates if operation was succesfull</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <response code="409">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Users")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserImage(int userId)
        {
            _logger.LogInformation($"Llamada a la funcion DeleteUserImage en el controlador UserController");
            if (GroupId == null && UserId == null && UserRole == "Usuario-Manager")
                return Unauthorized(new Models.CommonResponse<GroupBasicDataResponse>(new ErrorProperties(401, NoAuthConstant)));

            var result = await _userManager.DeleteUserImage(userId, GroupId!.Value);

            if (result.Error != null)
                return StatusCode(result.Error.Code, new CommonResponse<bool>(result.Error));

            return Ok(new CommonResponse<bool>(true));
        }

    }
}
