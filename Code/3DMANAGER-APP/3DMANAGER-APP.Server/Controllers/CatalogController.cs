using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Catalog;
using _3DMANAGER_APP.Server.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using static _3DMANAGER_APP.Server.Models.Response;

namespace _3DMANAGER_APP.Server.Controllers
{
    [ApiController]
    [Route("api/v1/catalogs")]
    public class CatalogController : BaseController
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService, ILogger<CatalogController> logger) : base(logger)
        {
            _catalogService = catalogService;
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
        [HttpGet("filament-types")]
        public CommonResponse<List<CatalogResponse>> GetFilamentType()
        {
            _logger.LogInformation($"Llamada a la funcion GetFilamentType en el controlador CatalogController");
            List<CatalogResponse> catalog = _catalogService.GetFilamentType();
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
        [HttpGet("print-states")]
        public CommonResponse<List<CatalogResponse>> GetPrintState()
        {
            _logger.LogInformation($"Llamada a la funcion GetPrintState en el controlador CatalogController");
            List<CatalogResponse> catalog = _catalogService.GetPrintState();
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
        [ProducesResponseType(typeof(CommonResponse<List<CatalogResponse>>), StatusCodes.Status401Unauthorized)]
        [ApiVersionNeutral]
        [Tags("Catalogs")]
        [HttpGet("filaments")]
        public IActionResult GetFilamentCatalog()
        {
            _logger.LogInformation($"Llamada a la funcion GetFilamentCatalog en el controlador CatalogController");
            if (GroupId == null)
                return Unauthorized(new CommonResponse<bool>(new ErrorProperties(401, "No autenticado")));

            List<CatalogResponse> catalog = _catalogService.GetFilamentCatalog(GroupId.Value);
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
        [ProducesResponseType(typeof(CommonResponse<List<CatalogPrinterResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommonResponse<List<CatalogPrinterResponse>>), StatusCodes.Status401Unauthorized)]
        [ApiVersionNeutral]
        [Tags("Catalogs")]
        [HttpGet("printers")]
        public IActionResult GetPrinterCatalog()
        {
            _logger.LogInformation($"Llamada a la funcion GetPrinterCatalog en el controlador CatalogController");
            if (GroupId == null)
                return Unauthorized(new CommonResponse<bool>(new ErrorProperties(401, "No autenticado")));
            List<CatalogPrinterResponse> catalog = _catalogService.GetPrinterCatalog(GroupId.Value);
            return Ok(new CommonResponse<List<CatalogPrinterResponse>>(catalog));
        }

        /// <summary>
        /// A catalog of printer states
        /// </summary>
        /// <returns>A catalog of print states</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommonResponse<List<CatalogResponse>>), StatusCodes.Status200OK)]
        [ApiVersionNeutral]
        [Tags("Catalogs")]
        [HttpGet("printer-states")]
        public CommonResponse<List<CatalogResponse>> GetPrinterState()
        {
            _logger.LogInformation($"Llamada a la funcion GetPrinterState en el controlador CatalogController");
            List<CatalogResponse> catalog = _catalogService.GetPrinterState();
            return new CommonResponse<List<CatalogResponse>>(catalog);
        }

        /// <summary>
        /// A catalog of filament states
        /// </summary>
        /// <returns>A catalog of filament states</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommonResponse<List<CatalogResponse>>), StatusCodes.Status200OK)]
        [ApiVersionNeutral]
        [Tags("Catalogs")]
        [HttpGet("filament-states")]
        public CommonResponse<List<CatalogResponse>> GetFilamentState()
        {
            _logger.LogInformation($"Llamada a la funcion GetFilamentState en el controlador CatalogController");
            List<CatalogResponse> catalog = _catalogService.GetFilamentState();
            return new CommonResponse<List<CatalogResponse>>(catalog);
        }
    }
}
