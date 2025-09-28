using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg;

namespace _3DMANAGER.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        public ILogger<BaseController> _logger;
        
        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }
    }
}
