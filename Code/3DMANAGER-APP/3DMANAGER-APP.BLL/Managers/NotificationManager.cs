using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Notifications;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Notifications;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace _3DMANAGER_APP.BLL.Managers
{
    public class NotificationManager : INotificationManager
    {
        private readonly INotificationDbManager _notificationDbManager;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationManager> _logger;
        private readonly IEmailService _emailService;

        public NotificationManager(
            INotificationDbManager notificationDbManager,
            IMapper mapper,
            ILogger<NotificationManager> logger,
            IEmailService emailService)
        {
            _notificationDbManager = notificationDbManager;
            _mapper = mapper;
            _logger = logger;
            _emailService = emailService;
        }

        public bool CreateNotification(int userId, NotificationType type, string message, string? email, out BaseError? error)
        {
            error = null;

            NotificationDbObject dbObj = new()
            {
                NotificationUserId = userId,
                NotificationMessage = message,
                NotificationType = (int)type
            };

            bool responseDb = _notificationDbManager.InsertNotification(dbObj, out int newId);

            if (!responseDb)
            {
                error = new BaseError { code = (int)HttpStatusCode.InternalServerError, message = "Error al crear notificación" };
                return false;
            }

            if (email != null)
            {
                try
                {
                    _emailService.SendEmailAsync(email, $"Notificación: {type}", $"<p>{message}</p>");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error enviando email de notificación");
                }
            }

            return true;
        }

        public List<NotificationObject> GetUnreadNotifications(int userId, out BaseError? error)
        {
            error = null;

            List<NotificationDbObject> list = _notificationDbManager.GetUnreadNotifications(userId, out bool errorDb);

            if (errorDb)
            {
                error = new BaseError { code = 500, message = "Error al obtener notificaciones" };
                return new List<NotificationObject>();
            }

            return _mapper.Map<List<NotificationObject>>(list);
        }

        public bool NotificationMarkAsRead(int notificationId, out BaseError? error)
        {
            error = null;

            bool responseDb = _notificationDbManager.NotificationMarkAsRead(notificationId);

            if (!responseDb)
            {
                error = new BaseError { code = (int)HttpStatusCode.InternalServerError, message = "Error al marcar notificación como leída" };
                return false;
            }

            return true;
        }
    }

}
