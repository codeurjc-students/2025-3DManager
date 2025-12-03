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
                        list.Add(listResponse.Create(row));
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

        public bool PostFilament(FilamentRequestDbObject request, out int? error)
        {
            error = null;
            try
            {
                string procName = $"{ProcedurePrefix}_pr_FILAMENT_POST";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_GROUP_ID", MySqlDbType.Int32) { Value = request.GroupId });
                cmd.Parameters.Add(new MySqlParameter("P_FILAMENT_NAME", MySqlDbType.VarChar) { Value = request.FilamentName });
                cmd.Parameters.Add(new MySqlParameter("P_FILAMENT_TYPE", MySqlDbType.Int32) { Value = request.FilamentType });
                cmd.Parameters.Add(new MySqlParameter("P_FILAMENT_WEIGHT", MySqlDbType.Int32) { Value = request.FilamentWeight });
                cmd.Parameters.Add(new MySqlParameter("P_FILAMENT_COLOR", MySqlDbType.VarChar) { Value = request.FilamentColor });
                cmd.Parameters.Add(new MySqlParameter("P_FILAMENT_TEMPERATURE", MySqlDbType.Int32) { Value = request.FilamentTemperature });
                cmd.Parameters.Add(new MySqlParameter("P_FILAMENT_THICKNESS", MySqlDbType.Float) { Value = request.FilamentThickness });
                cmd.Parameters.Add(new MySqlParameter("P_FILAMENT_COST", MySqlDbType.Decimal) { Value = request.FilamentCost });
                cmd.Parameters.Add(new MySqlParameter("P_FILAMENT_LENGHT", MySqlDbType.Int32) { Value = request.FilamentLenght });
                cmd.Parameters.Add(new MySqlParameter("P_FILAMENT_DESCRIPTION", MySqlDbType.VarChar) { Value = request.FilamentDescription });

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
                string msg = "Error al crear un filamento en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return false;
            }
            catch (Exception ex)
            {
                string msg = "Error al crear un filamento en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return false;
            }
        }
    }

}
