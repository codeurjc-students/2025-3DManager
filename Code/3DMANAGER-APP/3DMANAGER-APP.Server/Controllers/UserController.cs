using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.Server.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using static _3DMANAGER_APP.Server.Models.Response;

namespace _3DMANAGER_APP.Server.Controllers
{
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager, ILogger<UserController> logger) : base(logger)
        {
            _userManager = userManager;
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
        [Tags("User")]
        [HttpPost]
        public CommonResponse<bool> PostNewUser(UserObject user)
        {
            var response = _userManager.PostNewUser(user, out BaseError error);
            if (error != null)
            {
                return new CommonResponse<bool>(new ErrorProperties(error.code, error.message));
            }
            return new CommonResponse<bool>(true);
        }

    }
}
