using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Group;
using _3DMANAGER_APP.DAL.Models.User;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;

namespace _3DMANAGER_APP.DAL.Repositories
{
    public class GroupRepository : MySQLManager, IGroupRepository
    {
        private const string ErrorConstant = "CodigoError";
        private const string UserContant = "P_CD_USER";
        private const string GroupContant = "P_CD_GROUP";
        public GroupRepository(IDataSource<MySqlConnection> dataSourceFactory, ILogger<GroupRepository> logger) : base(dataSourceFactory, logger)
        {
        }

        public bool PostNewGroup(GroupRequestDbObject request, out int? errorDb)
        {
            errorDb = null;
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

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
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
                errorDb = 500;
                string msg = "Error al crear un grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                errorDb = 500;
                string msg = "Error al crear un grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public List<GroupInvitationDbObject> GetGroupInvitations(int userId, out int? errorDb)
        {
            errorDb = 0;
            try
            {
                List<GroupInvitationDbObject> list = new List<GroupInvitationDbObject>();
                string procName = $"{ProcedurePrefix}_pr_GROUP_INVITATIONS";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter(UserContant, MySqlDbType.VarChar) { Value = userId });

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
                string msg = $"Error al devolver el listado de invitaciones de grupo para el usuario {userId} de en BBDD";
                Logger.LogError(ex, msg);
                return new List<GroupInvitationDbObject>();
            }
            catch (Exception ex)
            {
                string msg = $"Error al devolver el listado de invitaciones de grupo para el usuario {userId} de en BBDD";
                Logger.LogError(ex, msg);
                return new List<GroupInvitationDbObject>();
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

                cmd.Parameters.Add(new MySqlParameter(GroupContant, MySqlDbType.Int32) { Value = groupId });
                cmd.Parameters.Add(new MySqlParameter("P_IT_ACCEPTED", MySqlDbType.Bit) { Value = isAccepted });
                cmd.Parameters.Add(new MySqlParameter(UserContant, MySqlDbType.Int32) { Value = userId });

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
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
                errorDb = 500;
                string msg = $"Error al aceptar invitacion de grupo {groupId} para el usuario {userId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                errorDb = 500;
                string msg = $"Error al aceptar invitacion de grupo {groupId} para el usuario {userId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public GroupBasicDataResponseDbObject GetGroupBasicData(int groupId)
        {
            try
            {
                GroupBasicDataResponseDbObject response = new GroupBasicDataResponseDbObject();
                string procName = $"{ProcedurePrefix}_pr_GROUP_BASIC_DATA_GET";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter(GroupContant, MySqlDbType.Int32) { Value = groupId });

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
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
                string msg = $"Error al aceptar invitacion de grupo {groupId} en BBDD";
                Logger.LogError(ex, msg);
                return new GroupBasicDataResponseDbObject();
            }
            catch (Exception ex)
            {
                string msg = $"Error al aceptar invitacion de grupo {groupId} en BBDD";
                Logger.LogError(ex, msg);
                return new GroupBasicDataResponseDbObject();
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
                cmd.Parameters.Add(new MySqlParameter(GroupContant, MySqlDbType.Int32) { Value = groupId });

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
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
                string msg = $"Error al actualizar un grupo {groupId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = $"Error al actualizar un grupo {groupId} en BBDD";
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

                cmd.Parameters.Add(new MySqlParameter(UserContant, MySqlDbType.Int32) { Value = userId });

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                var error = Convert.ToInt32(errorParam.Value);
                return error == 0;

            }
            catch (MySqlException ex)
            {
                string msg = $"Error del usuario {userId} al abandonar un grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = $"Error del usuario {userId} al abandonar un grupo en BBDD";
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

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                var error = Convert.ToInt32(errorParam.Value);
                return error == 0;
            }
            catch (MySqlException ex)
            {
                string msg = $"Error al expulsar al usuario {userKickedId} de un grupo en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = $"Error al expulsar al usuario {userKickedId} de un grupo en BBDD";
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

                cmd.Parameters.Add(new MySqlParameter(GroupContant, MySqlDbType.Int32) { Value = groupId });
                cmd.Parameters.Add(new MySqlParameter(UserContant, MySqlDbType.Int32) { Value = userId });

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                var error = Convert.ToInt32(errorParam.Value);
                if (error != 0)
                {
                    return false;
                }
                return true;
            }
            catch (MySqlException ex)
            {
                string msg = $"Error al eliminar el grupo {groupId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = $"Error al eliminar el grupo {groupId} en BBDD";
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
                cmd.Parameters.Add(new MySqlParameter(GroupContant, MySqlDbType.Int32) { Value = groupId });
                cmd.Parameters.Add(new MySqlParameter(UserContant, MySqlDbType.Int32) { Value = userId });

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                var error = Convert.ToInt32(errorParam.Value);
                return error == 0;
            }
            catch (MySqlException ex)
            {
                string msg = $"Error al tranferir a el usuario {userId} el rol de manager de el grupo {groupId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = $"Error al tranferir a el usuario {userId} el rol de manager de el grupo {groupId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public GroupDashboardDataDbObject GetGroupDashboardData(int groupId)
        {
            try
            {
                GroupDashboardDataDbObject response = new GroupDashboardDataDbObject();
                string procName = $"{ProcedurePrefix}_pr_GROUP_DASHBOARD_DATA_GET";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter(GroupContant, MySqlDbType.Int32) { Value = groupId });

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    response = GroupDashboardDataDbObject.Create(ds.Tables[0].Rows[0]);
                    response.GroupId = groupId;
                }
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        GroupPrinterHoursDbObject listMember = new GroupPrinterHoursDbObject();
                        response.GroupPrinterHours.Add(listMember.Create(row));
                    }
                }

                return response;
            }
            catch (MySqlException ex)
            {
                string msg = $"Error al obtener la información de grupo {groupId} para el dashboard en BBDD";
                Logger.LogError(ex, msg);
                return new GroupDashboardDataDbObject();
            }
            catch (Exception ex)
            {
                string msg = $"Error al obtener la información de grupo {groupId} para el dashboard en BBDD";
                Logger.LogError(ex, msg);
                return new GroupDashboardDataDbObject();
            }
        }
    }
}
