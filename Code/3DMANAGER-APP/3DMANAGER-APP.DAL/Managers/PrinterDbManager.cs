using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models;
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

        public bool PostPrinter(PrinterRequestDbObject request, out int? error)
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
                cmd.Parameters.Add(new MySqlParameter("P_PRINTER_DESCRIPTION", MySqlDbType.VarChar) { Value = request.PrinterModel });
                cmd.Parameters.Add(new MySqlParameter("P_PRINTER_MODEL", MySqlDbType.VarChar) { Value = request.PrinterDescription });

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
                string msg = "Error al crear un impresora en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return false;
            }
            catch (Exception ex)
            {
                string msg = "Error al crear un impresora en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return false;
            }
        }
    }

}
