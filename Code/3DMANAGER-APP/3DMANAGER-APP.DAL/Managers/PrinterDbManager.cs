using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models;
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

                //cmd.Parameters.Add(new MySqlParameter("@CD_PARTE", MySqlDbType.Guid) { Value = reportCode });

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
    }

}
