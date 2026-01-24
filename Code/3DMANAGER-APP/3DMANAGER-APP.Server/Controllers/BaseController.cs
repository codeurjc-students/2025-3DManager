using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _3DMANAGER_APP.Server.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        public ILogger<BaseController> _logger;

        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }
        protected int UserId =>
        int.Parse(User.FindFirst("userId")!.Value);

        protected int GroupId =>
            int.Parse(User.FindFirst("groupId")!.Value);

        protected string UserRole =>
            User.FindFirst(ClaimTypes.Role)?.Value ?? "";
    }
}
