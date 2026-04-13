using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.File;
using _3DMANAGER_APP.DAL.Models.Print;
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
        private const string ErrorConstant = "CodigoError";

        public int PostNewUser(UserCreateRequestDbObject user, out int? error)
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

                var errorDb = Convert.ToInt32(errorParam.Value);
                if (errorDb != 0)
                {
                    error = errorDb;
                    return 0;
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0].Field<int>("3DMANAGER_USER_ID");
                }

                return 0;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al crear un usuario en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return 0;
            }
            catch (Exception ex)
            {
                string msg = "Error al crear un usuario en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return 0;
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
                    return UserDbObject.Create(ds.Tables[0].Rows[0]);
                }

                return user;
            }
            catch (MySqlException ex)
            {
                string msg = $"Error al acceder con un usuario {userName} en BBDD";
                Logger.LogError(ex, msg);
                return new UserDbObject();
            }
            catch (Exception ex)
            {
                string msg = $"Error al acceder con un usuario {userName} en BBDD";
                Logger.LogError(ex, msg);
                return new UserDbObject();
            }
        }

        public List<UserListResponseDbObject> GetUserList(int group, out bool error)
        {
            try
            {
                error = false;
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
                error = true;
                string msg = $"Error al devolver el listado de usuarios del grupo {group} en BBDD";
                Logger.LogError(ex, msg);
                return new List<UserListResponseDbObject>();
            }
            catch (Exception ex)
            {
                error = true;
                string msg = $"Error al devolver el listado de usuarios del grupo {group} en BBDD";
                Logger.LogError(ex, msg);
                return new List<UserListResponseDbObject>();
            }
        }

        public List<UserListResponseDbObject> GetUserInvitationList(string? filter, out bool error)
        {
            try
            {
                error = false;
                List<UserListResponseDbObject> list = new List<UserListResponseDbObject>();
                string procName = $"{ProcedurePrefix}_pr_USER_INVITATION_LIST";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_FILTER", MySqlDbType.VarChar) { Value = filter });

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    error = false;
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
                error = true;
                string msg = "Error al devolver el listado de usuarios para invitar  en BBDD";
                Logger.LogError(ex, msg);
                return new List<UserListResponseDbObject>();
            }
            catch (Exception ex)
            {
                error = true;
                string msg = "Error al devolver el listado de usuarios para invitar de en BBDD";
                Logger.LogError(ex, msg);
                return new List<UserListResponseDbObject>();
            }
        }

        public bool PostUserInvitation(int groupId, int userId, out int? error)
        {
            error = null;
            try
            {
                string procName = $"{ProcedurePrefix}_pr_USER_INVITATION";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_GROUP", MySqlDbType.Int32) { Value = groupId });
                cmd.Parameters.Add(new MySqlParameter("P_CD_USER", MySqlDbType.Int32) { Value = userId });
                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);
                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                var errorDb = Convert.ToInt32(errorParam.Value);
                if (errorDb != 0)
                {
                    error = errorDb;
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
                error = 500;
                string msg = $"Error al invitar al usuario {userId} al grupo {groupId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                error = 500;
                string msg = $"Error al invitar al usuario {userId} al grupo {groupId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public bool UpdateUserImageData(int userId, FileResponseDbObject image)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_USER_POST_IMAGE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_KEY", MySqlDbType.VarChar) { Value = image.FileKey });
                cmd.Parameters.Add(new MySqlParameter("P_URL", MySqlDbType.VarChar) { Value = image.FileUrl });
                cmd.Parameters.Add(new MySqlParameter("P_USER_ID", MySqlDbType.Int32) { Value = userId });
                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException ex)
            {
                string msg = $"Error al guardar los datos de la imagen del usuario {userId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = $"Error al guardar los datos de la imagen del usuario {userId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public UserDbObject GetUserById(int userId)
        {
            try
            {
                var user = new UserDbObject();
                string procName = $"{ProcedurePrefix}_pr_USER_GET_BY_ID";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_USER", MySqlDbType.Int32) { Value = userId });

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return UserDbObject.Create(ds.Tables[0].Rows[0]);
                }

                return user;
            }
            catch (MySqlException ex)
            {
                string msg = $"Error al obtener el usuario por id : {userId} en BBDD";
                Logger.LogError(ex, msg);
                return new UserDbObject();
            }
            catch (Exception ex)
            {
                string msg = $"Error al obtener el usuario por id : {userId} en BBDD";
                Logger.LogError(ex, msg);
                return new UserDbObject();
            }
        }

        public int GetGroupIdByUserId(int userId)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_USER_GROUP_GET_BY_ID";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_USER", MySqlDbType.Int32) { Value = userId });

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0].Field<int>("3DMANAGER_USER_GROUP_ID");
                }

                return 0;
            }
            catch (MySqlException ex)
            {
                string msg = $"Error al obtener el id de grupo del usuario {userId} en BBDD";
                Logger.LogError(ex, msg);
                return 0;
            }
            catch (Exception ex)
            {
                string msg = $"Error al obtener el id de grupo del usuario {userId} en BBDD";
                Logger.LogError(ex, msg);
                return 0;
            }
        }

        public bool UpdateUser(UserUpdateRequestDbObject requestDb, out int? error)
        {
            error = null;
            try
            {
                string procName = $"{ProcedurePrefix}_pr_USER_UPDATE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_GROUP", MySqlDbType.Int32) { Value = requestDb.GroupId });
                cmd.Parameters.Add(new MySqlParameter("P_CD_USER", MySqlDbType.Int32) { Value = requestDb.UserId });
                cmd.Parameters.Add(new MySqlParameter("P_DS_NAME", MySqlDbType.VarChar) { Value = requestDb.UserName });
                cmd.Parameters.Add(new MySqlParameter("P_DS_EMAIL", MySqlDbType.VarChar) { Value = requestDb.UserEmail });

                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                int errorDb = Convert.ToInt32(errorParam.Value);
                if (errorDb != 0)
                {
                    error = errorDb;
                    return false;
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0].Field<long>("Total") > 0;
                }

                return false;
            }
            catch (MySqlException ex)
            {
                error = 500;
                string msg = $"Error al actualizar el perfil del usuario {requestDb.UserId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                error = 500;
                string msg = $"Error al actualizar el perfil del usuario {requestDb.UserId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public UserDetailDbObject GetUserDetail(int userId)
        {
            try
            {
                UserDetailDbObject response = new UserDetailDbObject();
                string procName = $"{ProcedurePrefix}_pr_USER_DETAIL_GET";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_USER", MySqlDbType.VarChar) { Value = userId });

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    response = new UserDetailDbObject();
                    return response.Create(ds.Tables[0].Rows[0]);
                }

                return response;
            }
            catch (MySqlException ex)
            {
                string msg = $"Error al devolver el detalle de usuario {userId} de en BBDD";
                Logger.LogError(ex, msg);
                return new UserDetailDbObject();
            }
            catch (Exception ex)
            {
                string msg = $"Error al devolver el detalle de usuario {userId} de en BBDD";
                Logger.LogError(ex, msg);
                return new UserDetailDbObject();
            }
        }

        public FileResponseDbObject GetUserImageData(int userId, out bool error)
        {
            error = false;
            FileResponseDbObject responseDb = new FileResponseDbObject();
            try
            {
                string procName = $"{ProcedurePrefix}_pr_USER_GET_IMAGE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_USER", MySqlDbType.Int32) { Value = userId });
                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    error = false;
                    return responseDb.Create(ds.Tables[0].Rows[0]);
                }

                return responseDb;
            }
            catch (MySqlException ex)
            {
                error = true;
                string msg = $"Error al guardar los datos de la imagen en el usuario {userId} BBDD";
                Logger.LogError(ex, msg);
                return new FileResponseDbObject();
            }
            catch (Exception ex)
            {
                error = true;
                string msg = $"Error al guardar los datos de la imagen en el usuario {userId} BBDD";
                Logger.LogError(ex, msg);
                return new FileResponseDbObject();
            }
        }

        public bool DeleteUserImageData(int userId)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_USER_DELETE_IMAGE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_USER", MySqlDbType.Int32) { Value = userId });
                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch (MySqlException ex)
            {
                string msg = $"Error al borrar los datos de la imagen en el usuario {userId} BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = $"Error al borrar los datos de la imagen en la usuario {userId} BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public DeletedDbObject DeleteUser(int userId, out int? error)
        {
            error = null;
            try
            {
                DeletedDbObject response = new DeletedDbObject { SuccesfullDelete = false };

                string procName = $"{ProcedurePrefix}_pr_USER_DELETE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_USER", MySqlDbType.Int32) { Value = userId });

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                var errorDb = Convert.ToInt32(errorParam.Value);
                if (errorDb != 0)
                {
                    error = errorDb;
                    response.Id = userId;
                    return response;
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    response = response.Create(ds.Tables[0].Rows[0]);
                    response.SuccesfullDelete = true;
                    return response;
                }

                return response;
            }
            catch (MySqlException ex)
            {
                string msg = $"Error al eliminar el usuario {userId} en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return new DeletedDbObject { SuccesfullDelete = false };
            }
            catch (Exception ex)
            {
                string msg = $"Error al eliminar el usuario {userId} en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return new DeletedDbObject { SuccesfullDelete = false };
            }
        }
    }
}
