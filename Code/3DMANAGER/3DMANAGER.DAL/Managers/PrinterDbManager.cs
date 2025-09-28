using _3DMANAGER.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using _3DMANAGER.DAL.Models;
using _3DMANAGER.DAL.Base;
using Microsoft.Extensions.Logging;

namespace _3DMANAGER.DAL.Managers
{
    public class PrinterDbManager : MySQLManager, IPrinterDbManager
    {
        public PrinterDbManager(IDataSource<MySqlConnection> dataSourceFactory, ILogger<PrinterDbManager> logger)
            : base(dataSourceFactory, logger) { }

        public List<PrinterDbObject> GetPrinterList(out int error)
        {
            error = 0;
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

                error = Convert.ToInt32(errorParam.Value);
                
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
                    return result;
                }

                throw new Exception("Error al obtener listado de impresoras");
            }
            catch (MySqlException ex)
            {
                Logger.LogError(ex, $"Error al obtener listado impresoras");
                error = 500;
                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error al obtener listado impresoras");
                error = 500;
                return null;
            }
        }
    }

}
