using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Catalog;
using _3DMANAGER_APP.Server.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using static _3DMANAGER_APP.Server.Models.Response;

namespace _3DMANAGER_APP.Server.Controllers
{
    [ApiController]
    [Route("api/v1/catalogs/[action]")]
    public class CatalogController : BaseController
    {
        private readonly ICatalogManager _catalogManager;

        public CatalogController(ICatalogManager catalogManager, ILogger<CatalogController> logger) : base(logger)
        {
            _catalogManager = catalogManager;
        }



        /// <summary>
        /// Return a catalog of filament Types
        /// </summary>
        /// <returns>A catalog of filaments types</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommonResponse<List<CatalogResponse>>), StatusCodes.Status200OK)]
        [ApiVersionNeutral]
        [Tags("Catalogs")]
        [HttpGet]
        public CommonResponse<List<CatalogResponse>> GetFilamentType()
        {
            List<CatalogResponse> catalog = _catalogManager.GetFilamentType();
            return new CommonResponse<List<CatalogResponse>>(catalog);
        }

        /// <summary>
        /// A catalog of print states
        /// </summary>
        /// <returns>A catalog of print states</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommonResponse<List<CatalogResponse>>), StatusCodes.Status200OK)]
        [ApiVersionNeutral]
        [Tags("Catalogs")]
        [HttpGet]
        public CommonResponse<List<CatalogResponse>> GetPrintState()
        {
            List<CatalogResponse> catalog = _catalogManager.GetPrintState();
            return new CommonResponse<List<CatalogResponse>>(catalog);
        }

        /// <summary>
        /// Return a catalog of filaments
        /// </summary>
        /// <returns>A catalog of filaments owns by a group</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommonResponse<List<CatalogResponse>>), StatusCodes.Status200OK)]
        [ApiVersionNeutral]
        [Tags("Catalogs")]
        [HttpGet]
        public IActionResult GetFilamentCatalog()
        {
            if (GroupId == null)
                return Unauthorized(new CommonResponse<bool>(new ErrorProperties(401, "No autenticado")));

            List<CatalogResponse> catalog = _catalogManager.GetFilamentCatalog(GroupId.Value);
            return Ok(new CommonResponse<List<CatalogResponse>>(catalog));
        }

        /// <summary>
        /// Return a catalog of Printers
        /// </summary>
        /// <returns>A catalog of printers owns by a group</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommonResponse<List<CatalogResponse>>), StatusCodes.Status200OK)]
        [ApiVersionNeutral]
        [Tags("Catalogs")]
        [HttpGet]
        public IActionResult GetPrinterCatalog()
        {
            if (GroupId == null)
                return Unauthorized(new CommonResponse<bool>(new ErrorProperties(401, "No autenticado")));
            List<CatalogResponse> catalog = _catalogManager.GetPrinterCatalog(GroupId.Value);
            return Ok(new CommonResponse<List<CatalogResponse>>(catalog));
        }
    }
}
