using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Notifications;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Notifications;
using _3DMANAGER_APP.DAL.Models.User;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace _3DMANAGER_APP.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationDbManager;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationService> _logger;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userDbManager;


        public NotificationService(
            INotificationRepository notificationDbManager,
            IMapper mapper,
            ILogger<NotificationService> logger,
            IEmailService emailService,
            IUserRepository userDbManager)
        {
            _notificationDbManager = notificationDbManager;
            _mapper = mapper;
            _logger = logger;
            _emailService = emailService;
            _userDbManager = userDbManager;
        }

        public bool CreateNotification(int userId, int userFromId, NotificationType type, string message, out BaseError? error)
        {
            error = null;

            NotificationDbObject dbObj = new()
            {
                NotificationUserId = userId,
                NotificationMessage = message,
                NotificationType = (int)type
            };

            bool responseDb = _notificationDbManager.InsertNotification(dbObj, out int newId);

            if (!responseDb && newId != 0)
            {
                error = new BaseError { code = (int)HttpStatusCode.InternalServerError, message = "Error al crear notificación" };
                return false;
            }

            UserDbObject user = _userDbManager.GetUserById(userId);
            UserDbObject userFrom = _userDbManager.GetUserById(userFromId);

            if (user.UserEmail != null)
            {
                try
                {
                    var (subject, html) = BuildNotificationEmail(type, user.UserName, userFrom.UserName, userFrom.GroupName!);
                    _emailService.SendEmailAsync(user.UserEmail, subject, html, true);

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
        public static (string subject, string html) BuildNotificationEmail(NotificationType type, string userName, string userNameFrom, string groupName)
        {
            string subject;
            string message;

            switch (type)
            {
                case NotificationType.FilamentWarning:
                    subject = "Aviso: Filamento bajo";
                    message = $"Hola {userName},<br><br>Uno de tus filamentos está por debajo del nivel recomendado. Te sugerimos revisarlo para evitar interrupciones en tus impresiones. Revisa la aplicación para encontrar los detalles del filamento.";
                    break;

                case NotificationType.GroupInvitation:
                    subject = "Invitación a un grupo";
                    message = $"Has sido invitado a unirte al grupo <strong>{groupName}</strong> por el administrador <strong>{userNameFrom}</strong>.";
                    break;

                case NotificationType.GroupExpulsion:
                    subject = "Expulsión del grupo";
                    message = $"Has sido expulsado del grupo <strong>{groupName}</strong> por el administrador <strong>{userNameFrom}</strong>.";
                    break;

                case NotificationType.PrintComment:
                    subject = "Nuevo comentario en tu impresión";
                    message = $"El usuario <strong>{userNameFrom}</strong> ha comentado en una de tus impresiones.";
                    break;

                default:
                    subject = "Notificación de 3DManager";
                    message = "Tienes una nueva notificación.";
                    break;
            }

            string html = BuildEmailHtml(subject, message);
            return (subject, html);
        }

        private static string BuildEmailHtml(string title, string message)
        {
            return $@"
                <!DOCTYPE html>
                <html lang=""es"">
                <head>
                <meta charset=""UTF-8"">
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        padding: 20px;
                    }}
                    .email-container {{
                        max-width: 600px;
                        margin: auto;
                        background: white;
                        border-radius: 8px;
                        overflow: hidden;
                        box-shadow: 0 2px 8px rgba(0,0,0,0.1);
                    }}
                    .header-container {{
                        background-color: #2b2b2b;
                        padding: 1rem 2rem;
                        text-align: center;
                    }}
                    .app-title {{
                        color: white;
                        font-weight: bold;
                        font-size: 2rem;
                        font-family: Impact, sans-serif;
                        margin: 0;
                    }}
                    .highlight {{
                        color: #f1c40f;
                    }}
                    .content {{
                        padding: 20px;
                        font-size: 1rem;
                        color: #333;
                    }}
                    .footer {{
                        padding: 15px;
                        text-align: center;
                        font-size: 0.9rem;
                        color: #777;
                    }}
                </style>
                </head>
                <body>
                <div class=""email-container"">
                    <header class=""header-container"">
                        <h1 class=""app-title""><span class=""highlight"">3D</span>MANAGER</h1>
                    </header>
                    <div class=""content"">
                        <h2>{title}</h2>
                        <p>{message}</p>
                    </div>
                    <div class=""footer"">
                        Este correo ha sido generado automáticamente por 3DManager. Por favor, no responda
                    </div>
                </div>
                </body>
                </html>";
        }


    }

}
