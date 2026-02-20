using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.File;
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

                cmd.Parameters.Add(new MySqlParameter("P_CD_GROUP", MySqlDbType.Int32) { Value = group });
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

        public int PostPrint(PrintRequestDbObject request, out int? error)
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

                var errorDb = Convert.ToInt32(errorParam.Value);
                if (errorDb != 0)
                {
                    error = errorDb;
                    return 0;
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0].Field<int>("3DMANAGER_3DPRINT_ID");
                }


                return 0;
            }
            catch (MySqlException ex)
            {
                string msg = "Error al crear una impresión en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return 0;
            }
            catch (Exception ex)
            {
                string msg = "Error al crear una impresión en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return 0;
            }
        }

        public bool UpdatePrintImageData(int printId, FileResponseDbObject image)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_PRINT_POST_IMAGE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_KEY", MySqlDbType.VarChar) { Value = image.FileKey });
                cmd.Parameters.Add(new MySqlParameter("P_URL", MySqlDbType.VarChar) { Value = image.FileUrl });
                cmd.Parameters.Add(new MySqlParameter("P_PRINT_ID", MySqlDbType.Int32) { Value = printId });
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

        public List<PrintListResponseDbObject> GetPrintListByType(int group, int pageNumber, int pageSize, int type, int id, out int totalItems)
        {
            totalItems = 0;
            try
            {
                List<PrintListResponseDbObject> list = new List<PrintListResponseDbObject>();
                string procName = "";
                switch (type)
                {
                    case 1:
                        procName = $"{ProcedurePrefix}_pr_PRINT_LIST_OF_PRINTER";
                        break;
                    case 2:
                        procName = $"{ProcedurePrefix}_pr_PRINT_LIST_OF_FILAMENT";
                        break;
                    case 3:
                        procName = $"{ProcedurePrefix}_pr_PRINT_LIST_OF_USER";
                        break;

                }
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_GROUP", MySqlDbType.Int32) { Value = group });
                cmd.Parameters.Add(new MySqlParameter("P_PAGE_NUMBER", MySqlDbType.Int32) { Value = pageNumber });
                cmd.Parameters.Add(new MySqlParameter("P_PAGE_SIZE", MySqlDbType.Int32) { Value = pageSize });
                cmd.Parameters.Add(new MySqlParameter("P_ID", MySqlDbType.Int32) { Value = id });

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
                string msg = "Error al devolver el listado de impresiones de detalle en BBDD";
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
    }

}
