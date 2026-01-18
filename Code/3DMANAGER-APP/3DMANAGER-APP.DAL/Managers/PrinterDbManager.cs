using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models;
using _3DMANAGER_APP.DAL.Models.File;
using _3DMANAGER_APP.DAL.Models.Printer;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net;

namespace _3DMANAGER_APP.DAL.Managers
{
    public class PrinterDbManager : MySQLManager, IPrinterDbManager
    {
        public PrinterDbManager(IDataSource<MySqlConnection> dataSourceFactory, ILogger<PrinterDbManager> logger)
            : base(dataSourceFactory, logger) { }

        public List<PrinterDbObject> GetPrinterList(out ErrorDbObject error)
        {
            error = null;
            int errorCode = 0;
            List<PrinterDbObject> result = new List<PrinterDbObject>();
            try
            {
                string procName = $"{ProcedurePrefix}_pr_PRINTER_LIST_GET";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };


                var errorParam = CreateReturnValueParameter("@CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                errorCode = Convert.ToInt32(errorParam.Value);
                if (errorCode != 0)
                {
                    throw new Exception("Error al obtener listado de impresoras");
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string value = row.Field<string>("3DMANAGER_PRINTER_NAME");
                        if (!string.IsNullOrEmpty(value))
                        {
                            result.Add(new PrinterDbObject() { PrinterName = value });
                        }
                    }
                }

                return result;
            }
            catch (MySqlException ex)
            {
                Logger.LogError(ex, $"Error al obtener listado impresoras de BBDD");
                error = new ErrorDbObject() { code = (int)HttpStatusCode.InternalServerError, message = "Error al obtener las impresoras de BBDD" };
                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error al obtener listado impresoras de BBDD");
                error = new ErrorDbObject() { code = (int)HttpStatusCode.InternalServerError, message = "Error al obtener las impresoras de BBDD" };
                return null;
            }
        }

        public int PostPrinter(PrinterRequestDbObject request, out int? error)
        {
            error = null;
            try
            {
                string procName = $"{ProcedurePrefix}_pr_PRINTER_POST";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_GROUP_ID", MySqlDbType.Int32) { Value = request.GroupId });
                cmd.Parameters.Add(new MySqlParameter("P_PRINTER_NAME", MySqlDbType.VarChar) { Value = request.PrinterName });
                cmd.Parameters.Add(new MySqlParameter("P_PRINTER_DESCRIPTION", MySqlDbType.VarChar) { Value = request.PrinterDescription });
                cmd.Parameters.Add(new MySqlParameter("P_PRINTER_MODEL", MySqlDbType.VarChar) { Value = request.PrinterModel });
                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                error = Convert.ToInt32(errorParam.Value);
                if (error != 0)
                {
                    return 0;
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0].Field<int>("3DMANAGER_PRINTER_ID");
                }

                return 0;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al crear un impresora en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return 0;
            }
            catch (Exception ex)
            {
                string msg = "Error al crear un impresora en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return 0;
            }
        }

        public List<PrinterListDbObject> GetPrinterDashboardList(int group)
        {
            try
            {
                List<PrinterListDbObject> list = new List<PrinterListDbObject>();
                string procName = $"{ProcedurePrefix}_pr_PRINTER_LIST";
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
                        PrinterListDbObject listResponse = new PrinterListDbObject();
                        list.Add(listResponse.Create(row));
                    }
                }

                return list;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al devolver el listado de impresoras de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
            catch (Exception ex)
            {
                string msg = "Error al devolver el listado de impresoras de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
        }

        public bool UpdatePrinterImageData(int printerId, FileResponseDbObject image)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_PRINTER_POST_IMAGE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_KEY", MySqlDbType.VarChar) { Value = image.FileKey });
                cmd.Parameters.Add(new MySqlParameter("P_URL", MySqlDbType.VarChar) { Value = image.FileUrl });
                cmd.Parameters.Add(new MySqlParameter("P_PRINTER_ID", MySqlDbType.Int32) { Value = printerId });
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
                string msg = "Error al guardar los datos de la imagen en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = "Error al guardar los datos de la imagen en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }
    }

}
