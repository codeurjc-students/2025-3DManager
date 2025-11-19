using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Filament;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;

namespace _3DMANAGER_APP.DAL.Managers
{
    public class FilamentDbManager : MySQLManager, IFilamentDbManager
    {
        public FilamentDbManager(IDataSource<MySqlConnection> dataSourceFactory, ILogger<FilamentDbManager> logger)
            : base(dataSourceFactory, logger)
        {
        }

        public List<FilamentListResponseDbObject> GetFilamentList(int group)
        {
            try
            {
                List<FilamentListResponseDbObject> list = new List<FilamentListResponseDbObject>();
                string procName = $"{ProcedurePrefix}_pr_FILAMENT_LIST";
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
                        FilamentListResponseDbObject listResponse = new FilamentListResponseDbObject();
                        list.Add(listResponse.Create(ds.Tables[0].Rows[0]));
                    }
                }

                return list;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al devolver el listado de filamentos de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
            catch (Exception ex)
            {
                string msg = "Error al devolver el listado de filamentos de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
        }
    }

}
