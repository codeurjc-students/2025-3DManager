using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Catalog;
using _3DMANAGER_APP.Server.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace _3DMANAGER_APP.Server.Controllers
{
    [ApiController]
    public class CatalogController : BaseController
    {
        private readonly ICatalogManager _catalogManager;

        public CatalogController(ICatalogManager catalogManager, ILogger<CatalogController> logger) : base(logger)
        {
            _catalogManager = catalogManager;
        }



        /// <summary>
        /// Return a user list
        /// </summary>
        /// <returns>A catalog of filaments types</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="400">Conflicto en servidor</response>
        /// <responde code="500">Ocurrio un error en el servidor</responde>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommonResponse<List<CatalogResponse>>), StatusCodes.Status200OK)]
        [ApiVersionNeutral]
        [Tags("Catalog")]
        [HttpGet]
        public CommonResponse<List<CatalogResponse>> GetFilamentType()
        {
            List<CatalogResponse> catalog = _catalogManager.GetFilamentType();
            return new CommonResponse<List<CatalogResponse>>(catalog);
        }
    }
}
