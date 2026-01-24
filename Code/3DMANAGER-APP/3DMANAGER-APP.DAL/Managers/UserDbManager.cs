using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.User;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;

namespace _3DMANAGER_APP.DAL.Managers
{
    public class UserDbManager : MySQLManager, IUserDbManager
    {
        public UserDbManager(IDataSource<MySqlConnection> dataSourceFactory, ILogger<UserDbManager> logger)
            : base(dataSourceFactory, logger)
        {
        }

        public bool PostNewUser(UserCreateRequestDbObject user, out int? error)
        {
            error = null;
            try
            {
                string procName = $"{ProcedurePrefix}_pr_USER_POST";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_DS_USER_NAME", MySqlDbType.VarChar) { Value = user.UserName });
                cmd.Parameters.Add(new MySqlParameter("P_DS_USER_EMAIL", MySqlDbType.VarChar) { Value = user.UserEmail });
                cmd.Parameters.Add(new MySqlParameter("P_DS_USER_PASSWORD", MySqlDbType.VarChar) { Value = user.UserPassword });

                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                error = Convert.ToInt32(errorParam.Value);
                if (error != 0)
                {
                    return false;
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al crear un usuario en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return false;
            }
            catch (Exception ex)
            {
                string msg = "Error al crear un usuario en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return false;
            }
        }

        public UserDbObject Login(string userName)
        {
            try
            {
                var user = new UserDbObject();
                string procName = $"{ProcedurePrefix}_pr_USER_LOGIN";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_DS_USER_NAME", MySqlDbType.VarChar) { Value = userName });

                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return user.Create(ds.Tables[0].Rows[0]);
                }

                return null;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al acceder con un usuario en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
            catch (Exception ex)
            {
                string msg = "Error al acceder con un usuario en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
        }

        public List<UserListResponseDbObject> GetUserList(int group)
        {
            try
            {
                List<UserListResponseDbObject> list = new List<UserListResponseDbObject>();
                string procName = $"{ProcedurePrefix}_pr_USER_LIST";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_GROUP", MySqlDbType.VarChar) { Value = group });

                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        UserListResponseDbObject listResponse = new UserListResponseDbObject();
                        list.Add(listResponse.Create(row));
                    }
                }

                return list;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al devolver el listado de usuarios de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
            catch (Exception ex)
            {
                string msg = "Error al devolver el listado de usuarios de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
        }

        public List<UserListResponseDbObject> GetUserInvitationList()
        {
            try
            {
                List<UserListResponseDbObject> list = new List<UserListResponseDbObject>();
                string procName = $"{ProcedurePrefix}_pr_USER_INVITATION_LIST";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        UserListResponseDbObject listResponse = new UserListResponseDbObject();
                        list.Add(listResponse.Create(row));
                    }
                }

                return list;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al devolver el listado de usuarios para invitar de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
            catch (Exception ex)
            {
                string msg = "Error al devolver el listado de usuarios para invitar de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
        }

        public void PostUserInvitation(int groupId, int userId)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_USER_INVITATION";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_GROUP", MySqlDbType.Int32) { Value = groupId });
                cmd.Parameters.Add(new MySqlParameter("P_CD_USER", MySqlDbType.Int32) { Value = userId });

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

            }
            catch (MySqlException ex)
            {
                string msg = "Error al devolver el listado de usuarios de en BBDD";
                Logger.LogError(ex, msg);
            }
            catch (Exception ex)
            {
                string msg = "Error al devolver el listado de usuarios de en BBDD";
                Logger.LogError(ex, msg);
            }
        }
    }

}
