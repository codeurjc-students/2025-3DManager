using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Filament;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;

namespace _3DMANAGER_APP.DAL.Managers
{
    public class CatalogDbManager : MySQLManager, ICatalogDbManager
    {
        public CatalogDbManager(IDataSource<MySqlConnection> dataSourceFactory, ILogger<CatalogDbManager> logger)
            : base(dataSourceFactory, logger)
        {
        }

        public List<CatalogResponseDbObject> GetFilamentType()
        {
            try
            {
                List<CatalogResponseDbObject> list = new List<CatalogResponseDbObject>();
                string procName = $"{ProcedurePrefix}_pr_C_FILAMENT_TYPE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };


                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        CatalogResponseDbObject listResponse = new CatalogResponseDbObject();
                        list.Add(listResponse.Create(row));
                    }
                }

                return list;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al devolver el listado de tipos de filamentos de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
            catch (Exception ex)
            {
                string msg = "Error al devolver el listado de tipo de filamentos de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
        }

        public List<CatalogResponseDbObject> GetPrintState()
        {
            try
            {
                List<CatalogResponseDbObject> list = new List<CatalogResponseDbObject>();
                string procName = $"{ProcedurePrefix}_pr_C_PRINT_STATE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };


                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        CatalogResponseDbObject listResponse = new CatalogResponseDbObject();
                        list.Add(listResponse.Create(row));
                    }
                }

                return list;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al devolver el listado de tipos de estado de impresiones de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
            catch (Exception ex)
            {
                string msg = "Error al devolver el listado de tipos de estado de impresiones de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
        }

        public List<CatalogResponseDbObject> GetFilamentCatalog(int groupId)
        {
            try
            {
                List<CatalogResponseDbObject> list = new List<CatalogResponseDbObject>();
                string procName = $"{ProcedurePrefix}_pr_C_FILAMENT";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_GROUP", MySqlDbType.VarChar) { Value = groupId });
                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        CatalogResponseDbObject listResponse = new CatalogResponseDbObject();
                        list.Add(listResponse.Create(row));
                    }
                }

                return list;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al devolver el catalogo de filamentos de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
            catch (Exception ex)
            {
                string msg = "Error al devolver el catalogo de filamentos de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
        }

        public List<CatalogResponseDbObject> GetPrinterCatalog(int groupId)
        {
            try
            {
                List<CatalogResponseDbObject> list = new List<CatalogResponseDbObject>();
                string procName = $"{ProcedurePrefix}_pr_C_PRINTER";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_GROUP", MySqlDbType.VarChar) { Value = groupId });
                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        CatalogResponseDbObject listResponse = new CatalogResponseDbObject();
                        list.Add(listResponse.Create(row));
                    }
                }

                return list;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al devolver el catalogo de filamentos de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
            catch (Exception ex)
            {
                string msg = "Error al devolver el catalogo de filamentos de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
        }

        public List<CatalogResponseDbObject> GetPrinterState()
        {
            try
            {
                List<CatalogResponseDbObject> list = new List<CatalogResponseDbObject>();
                string procName = $"{ProcedurePrefix}_pr_C_PRINTER_STATE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };


                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        CatalogResponseDbObject listResponse = new CatalogResponseDbObject();
                        list.Add(listResponse.Create(row));
                    }
                }

                return list;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al devolver el listado de tipos de estado de impresoras de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
            catch (Exception ex)
            {
                string msg = "Error al devolver el listado de tipos de estado de impresoras de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
        }
    }

}
