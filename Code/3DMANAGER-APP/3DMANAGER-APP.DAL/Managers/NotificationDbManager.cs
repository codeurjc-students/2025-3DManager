using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Notifications;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;

namespace _3DMANAGER_APP.DAL.Managers
{
    public class NotificationDbManager : MySQLManager, INotificationDbManager
    {
        private const string ErrorConstant = "CodigoError";

        public NotificationDbManager(IDataSource<MySqlConnection> dataSourceFactory, ILogger<NotificationDbManager> logger)
            : base(dataSourceFactory, logger) { }

        public List<NotificationDbObject> GetUnreadNotifications(int userId, out bool error)
        {
            error = false;
            List<NotificationDbObject> list = new();
            try
            {
                string procName = $"{ProcedurePrefix}_pr_NOTIFICATION_GET_UNREAD";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_USER_ID", MySqlDbType.Int32) { Value = userId });

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (Convert.ToInt32(errorParam.Value) != 0)
                {
                    error = true;
                    return list;
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    NotificationDbObject obj = new();
                    list.Add(obj.Create(row));
                }

                return list;
            }
            catch (MySqlException ex)
            {
                error = true;
                Logger.LogError(ex, "Error al obtener notificaciones no leídas");
                return new List<NotificationDbObject>();
            }
            catch (Exception ex)
            {
                error = true;
                Logger.LogError(ex, "Error al obtener notificaciones no leídas");
                return new List<NotificationDbObject>();
            }
        }

        public bool InsertNotification(NotificationDbObject notification, out int newId)
        {
            newId = 0;
            try
            {
                string procName = $"{ProcedurePrefix}_pr_NOTIFICATION_INSERT";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_USER_ID", MySqlDbType.Int32) { Value = notification.NotificationUserId });
                cmd.Parameters.Add(new MySqlParameter("P_MESSAGE", MySqlDbType.VarChar) { Value = notification.NotificationMessage });
                cmd.Parameters.Add(new MySqlParameter("P_TYPE", MySqlDbType.Int32) { Value = notification.NotificationType });

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (Convert.ToInt32(errorParam.Value) != 0)
                    return false;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    newId = Convert.ToInt32(ds.Tables[0].Rows[0]["NOTIFICATION_ID"]);
                    return true;
                }

                return false;
            }
            catch (MySqlException ex)
            {
                Logger.LogError(ex, "Error al insertar notificación");
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error al insertar notificación");
                return false;
            }
        }

        public bool NotificationMarkAsRead(int notificationId)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_NOTIFICATION_MARK_READ";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_NOTIFICATION_ID", MySqlDbType.Int32) { Value = notificationId });

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                cmd.ExecuteNonQuery();

                return Convert.ToInt32(errorParam.Value) == 0;
            }
            catch (MySqlException ex)
            {
                Logger.LogError(ex, "Error al marcar notificación como leída");
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error al marcar notificación como leída");
                return false;
            }
        }
    }

}
