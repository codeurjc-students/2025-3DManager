using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Print;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;

namespace _3DMANAGER_APP.DAL.Managers
{
    public class PrintDbManager : MySQLManager, IPrintDbManager
    {
        public PrintDbManager(IDataSource<MySqlConnection> dataSourceFactory, ILogger<PrintDbManager> logger)
            : base(dataSourceFactory, logger)
        {
        }

        public List<PrintListResponseDbObject> GetPrintList(int group, int pageNumber, int pageSize, out int totalItems)
        {
            totalItems = 0;
            try
            {
                List<PrintListResponseDbObject> list = new List<PrintListResponseDbObject>();
                string procName = $"{ProcedurePrefix}_pr_PRINT_LIST";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_GROUP", MySqlDbType.VarChar) { Value = group });
                cmd.Parameters.Add(new MySqlParameter("P_PAGE_NUMBER", MySqlDbType.Int32) { Value = pageNumber });
                cmd.Parameters.Add(new MySqlParameter("P_PAGE_SIZE", MySqlDbType.Int32) { Value = pageSize });

                var errorParam = CreateReturnValueParameter("CodigoError", MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        PrintListResponseDbObject listResponse = new PrintListResponseDbObject();
                        list.Add(listResponse.Create(row));
                    }
                }

                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    totalItems = Convert.ToInt32(ds.Tables[1].Rows[0]["TOTAL_ITEMS"]);
                }

                return list;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al devolver el listado de impresiones de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
            catch (Exception ex)
            {
                string msg = "Error al devolver el listado de impresiones de en BBDD";
                Logger.LogError(ex, msg);
                return null;
            }
        }

        public bool PostPrint(PrintRequestDbObject request, out int? error)
        {
            error = null;
            try
            {
                string procName = $"{ProcedurePrefix}_pr_PRINT_POST";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_GROUP_ID", MySqlDbType.Int32) { Value = request.GroupId });
                cmd.Parameters.Add(new MySqlParameter("P_USER_ID", MySqlDbType.Int32) { Value = request.UserId });
                cmd.Parameters.Add(new MySqlParameter("P_PRINT_NAME", MySqlDbType.VarChar) { Value = request.PrintName });
                cmd.Parameters.Add(new MySqlParameter("P_PRINT_STATE", MySqlDbType.Int32) { Value = request.PrintState });
                cmd.Parameters.Add(new MySqlParameter("P_PRINT_PRINTER_ID", MySqlDbType.Int32) { Value = request.PrintPrinter });
                cmd.Parameters.Add(new MySqlParameter("P_PRINT_FILAMENT_ID", MySqlDbType.Int32) { Value = request.PrintFilament });
                cmd.Parameters.Add(new MySqlParameter("P_PRINT_DESCRIPTION", MySqlDbType.VarChar) { Value = request.PrintDescription });
                cmd.Parameters.Add(new MySqlParameter("P_PRINT_TIME", MySqlDbType.Int32) { Value = request.PrintTime });
                cmd.Parameters.Add(new MySqlParameter("P_PRINT_FILAMENT_USED", MySqlDbType.Decimal) { Value = request.PrintFilamentUsed });
                cmd.Parameters.Add(new MySqlParameter("P_PRINT_REAL_TIME", MySqlDbType.Int32) { Value = request.PrintRealTime });

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
                string msg = "Error al crear una impresión en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return false;
            }
            catch (Exception ex)
            {
                string msg = "Error al crear una impresión en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return false;
            }
        }
    }

}
