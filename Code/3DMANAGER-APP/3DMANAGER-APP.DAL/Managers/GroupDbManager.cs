using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
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
                cmd.Parameters.Add(new MySqlParameter("P_CD_USER_ID", MySqlDbType.VarChar) { Value = request.UserId });

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
    }
}
