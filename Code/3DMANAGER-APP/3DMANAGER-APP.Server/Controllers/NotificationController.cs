using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Notifications;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using static _3DMANAGER_APP.Server.Models.Response;

namespace _3DMANAGER_APP.Server.Controllers
{
    [ApiController]
    [Route("api/v1/notifications/[action]")]
    public class NotificationController : BaseController
    {
        private readonly INotificationManager _notificationManager;
        private const string NoAuthConstant = "No autenticado";
        public NotificationController(INotificationManager notificationManager, ILogger<NotificationController> logger) : base(logger)
        {
            _notificationManager = notificationManager;
        }

        /// <summary>
        /// Get non read it notifications
        /// </summary>
        /// <returns>List of notifications</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <response code="500">Error en servidor</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<List<NotificationObject>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<NotificationObject>>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<List<NotificationObject>>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Notifications")]
        [HttpGet]
        public IActionResult GetUnreadNotifications()
        {
            _logger.LogInformation("Llamada a GetUnreadNotifications en NotificationController");

            if (UserId == null)
                return Unauthorized(new Models.CommonResponse<List<NotificationObject>>(new ErrorProperties(401, NoAuthConstant)));

            var result = _notificationManager.GetUnreadNotifications(UserId.Value, out BaseError? error);

            if (error != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<List<NotificationObject>>(new ErrorProperties(
                error == null ? StatusCodes.Status500InternalServerError : error.code,
                error == null ? "Error al obtener el listado de notificaciones" : error.message)));
            }


            return Ok(new Models.CommonResponse<List<NotificationObject>>(result));
        }

        /// <summary>
        /// Mark a notification as read it
        /// </summary>
        /// <returns>Boolean that indicates if operation was succesfull</returns>
        /// <response code="200">Respuesta correcta</response>
        /// <response code="401">No autorizado</response>
        /// <response code="500">Error en servidor</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Models.CommonResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ApiVersionNeutral]
        [Tags("Notifications")]
        [HttpPost]
        public IActionResult NotificationMarkAsRead(int notificationId)
        {
            _logger.LogInformation("Llamada a NotificationMarkAsRead en NotificationController");

            if (UserId == null)
                return Unauthorized(new Models.CommonResponse<bool>(new ErrorProperties(401, NoAuthConstant)));

            bool response = _notificationManager.NotificationMarkAsRead(notificationId, out BaseError? error);

            if (!response)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Models.CommonResponse<bool>(new ErrorProperties(
                error == null ? StatusCodes.Status500InternalServerError : error.code,
                error == null ? "Error al marcan una notificación como leida" : error.message
            )));
            }


            return Ok(new Models.CommonResponse<bool>(true));
        }


    }
}
