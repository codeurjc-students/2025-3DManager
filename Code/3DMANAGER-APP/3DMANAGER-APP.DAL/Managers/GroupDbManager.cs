using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Group;
using _3DMANAGER_APP.DAL.Models.User;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;

namespace _3DMANAGER_APP.DAL.Managers
{
    public class GroupDbManager : MySQLManager, IGroupDbManager
    {
        public GroupDbManager(IDataSource<MySqlConnection> dataSourceFactory, ILogger<GroupDbManager> logger) : base(dataSourceFactory, logger)
        {
        }

        public bool PostNewGroup(GroupRequestDbObject request)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_GROUP_POST";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_DS_GROUP_NAME", MySqlDbType.VarChar) { Value = request.GroupName });
                cmd.Parameters.Add(new MySqlParameter("P_DS_GROUP_DESCRIPTION", MySqlDbType.VarChar) { Value = request.GroupDescription });
                cmd.Parameters.Add(new MySqlParameter("P_CD_USER_ID", MySqlDbType.Int32) { Value = request.UserId });

                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                var error = Convert.ToInt32(errorParam.Value);
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
                string msg = "Error al crear un grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = "Error al crear un grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public List<GroupInvitationDbObject> GetGroupInvitations(int userId)
        {
            try
            {
                List<GroupInvitationDbObject> list = new List<GroupInvitationDbObject>();
                string procName = $"{ProcedurePrefix}_pr_GROUP_INVITATIONS";
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
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        GroupInvitationDbObject listResponse = new GroupInvitationDbObject();
                        list.Add(listResponse.Create(row));
                    }
                }

                return list;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al devolver el listado de invitaciones de grupo de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
            catch (Exception ex)
            {
                string msg = "Error al devolver el listado de invitaciones de grupo de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
        }

        public bool PostAcceptInvitation(int groupId, bool isAccepted, int userId, out int? errorDb)
        {
            errorDb = null;
            try
            {
                string procName = $"{ProcedurePrefix}_pr_GROUP_ACCEPT_INVITATION";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_GROUP", MySqlDbType.Int32) { Value = groupId });
                cmd.Parameters.Add(new MySqlParameter("P_IT_ACCEPTED", MySqlDbType.Bit) { Value = isAccepted });
                cmd.Parameters.Add(new MySqlParameter("P_CD_USER", MySqlDbType.Int32) { Value = userId });

                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                var error = Convert.ToInt32(errorParam.Value);
                if (error != 0)
                {
                    errorDb = error;
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
                string msg = "Error al aceptar invitacion de grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = "Error al aceptar invitacion de grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public GroupBasicDataResponseDbObject GetGroupBasicData(int groupId)
        {
            try
            {
                GroupBasicDataResponseDbObject response = null;
                string procName = $"{ProcedurePrefix}_pr_GROUP_BASIC_DATA_GET";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_GROUP", MySqlDbType.Int32) { Value = groupId });

                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    response = new GroupBasicDataResponseDbObject();
                    response = response.Create(ds.Tables[0].Rows[0]);
                }
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        UserListResponseDbObject listMember = new UserListResponseDbObject();
                        response.GroupMembers.Add(listMember.Create(row));
                    }
                }

                return response;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al aceptar invitacion de grupo en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
            catch (Exception ex)
            {
                string msg = "Error al aceptar invitacion de grupo en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
        }

        public bool UpdateGroupData(GroupRequestDbObject request, int groupId)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_GROUP_UPDATE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_DS_GROUP_NAME", MySqlDbType.VarChar) { Value = request.GroupName });
                cmd.Parameters.Add(new MySqlParameter("P_DS_GROUP_DESCRIPTION", MySqlDbType.VarChar) { Value = request.GroupDescription });
                cmd.Parameters.Add(new MySqlParameter("P_CD_USER_ID", MySqlDbType.Int32) { Value = request.UserId });
                cmd.Parameters.Add(new MySqlParameter("P_CD_GROUP", MySqlDbType.Int32) { Value = groupId });

                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                var error = Convert.ToInt32(errorParam.Value);
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
                string msg = "Error al crear un grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = "Error al crear un grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public bool UpdateLeaveGroup(int userId)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_GROUP_LEAVE";
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

                var error = Convert.ToInt32(errorParam.Value);
                return error == 0;

            }
            catch (MySqlException ex)
            {
                string msg = "Error al abandonar un grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = "Error al abandonar un grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public bool UpdateMembership(int userKickedId)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_GROUP_KICK_USER";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_USER_KICKED", MySqlDbType.Int32) { Value = userKickedId });

                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                var error = Convert.ToInt32(errorParam.Value);
                return error == 0;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al expulsar a un usuario de un grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = "Error al expulsar a un usuario de un grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public bool DeleteGroup(int userId, int groupId)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_GROUP_DELETE";
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

                var error = Convert.ToInt32(errorParam.Value);
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
                string msg = "Error al eliminar un grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = "Error al eliminar un grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public bool TrasnferOwnership(int userId, int groupId, int newOwnerUserId)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_GROUP_TRANSFER_OWNERSHIP";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_NEW_OWNER", MySqlDbType.Int32) { Value = newOwnerUserId });
                cmd.Parameters.Add(new MySqlParameter("P_CD_GROUP", MySqlDbType.Int32) { Value = groupId });
                cmd.Parameters.Add(new MySqlParameter("P_CD_USER", MySqlDbType.Int32) { Value = userId });

                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                var error = Convert.ToInt32(errorParam.Value);
                return error == 0;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al tranferir a un usuario el rol de manager de un grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = "Error al expulsar a un usuario el rol de manager de un grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }
    }
}
