using _3DMANAGER_APP.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _3DMANAGER_APP.Server.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        public ILogger<BaseController> _logger;

        [FromServices]
        public IUserManager UserManager { get; set; } = default!;
        protected BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }
        protected int? UserId =>
            int.TryParse(User.FindFirst("userId")?.Value, out var id) ? id : null;

        protected int? GroupId
        {
            get
            {
                if (UserId == null) return null;
                return UserManager.GetGroupIdByUserId(UserId.Value);
            }
        }

        protected string UserRole =>
            User.FindFirst(ClaimTypes.Role)?.Value ?? "";

    }
}
