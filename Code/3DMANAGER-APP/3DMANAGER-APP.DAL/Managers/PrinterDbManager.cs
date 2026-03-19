using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models;
using _3DMANAGER_APP.DAL.Models.File;
using _3DMANAGER_APP.DAL.Models.Print;
using _3DMANAGER_APP.DAL.Models.Printer;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net;

namespace _3DMANAGER_APP.DAL.Managers
{
    public class PrinterDbManager : MySQLManager, IPrinterDbManager
    {

        private const string ErrorConstant = "CodigoError";
        private const string GroupParam = "P_CD_GROUP";
        private const string PrinterParam = "P_CD_PRINTER";

        public PrinterDbManager(IDataSource<MySqlConnection> dataSourceFactory, ILogger<PrinterDbManager> logger)
            : base(dataSourceFactory, logger) { }


        public List<PrinterDbObject> GetPrinterList(out ErrorDbObject error)
        {
            error = new ErrorDbObject();
            int errorCode = 0;
            List<PrinterDbObject> result = new List<PrinterDbObject>();
            try
            {
                string procName = $"{ProcedurePrefix}_pr_PRINTER_LIST_GET";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };


                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                errorCode = Convert.ToInt32(errorParam.Value);
                if (errorCode != 0)
                {
                    return result;
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string? value = row.Field<string>("3DMANAGER_PRINTER_NAME");
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
                return new List<PrinterDbObject>();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error al obtener listado impresoras de BBDD");
                error = new ErrorDbObject() { code = (int)HttpStatusCode.InternalServerError, message = "Error al obtener las impresoras de BBDD" };
                return new List<PrinterDbObject>();
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
                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
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
                string msg = $"Error al crear un impresora en grupo {request.GroupId} BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return 0;
            }
            catch (Exception ex)
            {
                string msg = $"Error al crear un impresora en grupo {request.GroupId} BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return 0;
            }
        }

        public List<PrinterListDbObject> GetPrinterDashboardList(int group, out bool error)
        {
            try
            {
                error = false;
                List<PrinterListDbObject> list = new List<PrinterListDbObject>();
                string procName = $"{ProcedurePrefix}_pr_PRINTER_LIST";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter(GroupParam, MySqlDbType.VarChar) { Value = group });

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    error = false;
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
                error = true;
                string msg = $"Error al devolver el listado de impresoras del grupo {group} en BBDD";
                Logger.LogError(ex, msg);
                return new List<PrinterListDbObject>();
            }
            catch (Exception ex)
            {
                error = true;
                string msg = $"Error al devolver el listado de impresoras del grupo {group} en BBDD";
                Logger.LogError(ex, msg);
                return new List<PrinterListDbObject>();
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
                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
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
                string msg = $"Error al guardar los datos de la imagen en la impresora {printerId} BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = $"Error al guardar los datos de la imagen en la impresora {printerId} BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public bool UpdatePrinter(PrinterDetailRequestDbObject requestDb)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_PRINTER_UPDATE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter(GroupParam, MySqlDbType.Int32) { Value = requestDb.GroupId });
                cmd.Parameters.Add(new MySqlParameter(PrinterParam, MySqlDbType.Int32) { Value = requestDb.PrinterId });
                cmd.Parameters.Add(new MySqlParameter("P_CD_STATE", MySqlDbType.Int32) { Value = requestDb.PrinterStateId });
                cmd.Parameters.Add(new MySqlParameter("P_DS_NAME", MySqlDbType.VarChar) { Value = requestDb.PrinterName });
                cmd.Parameters.Add(new MySqlParameter("P_DS_MODEL", MySqlDbType.VarChar) { Value = requestDb.PrinterModel });
                cmd.Parameters.Add(new MySqlParameter("P_DS_DESCRIPTION", MySqlDbType.VarChar) { Value = requestDb.PrinterDescription });

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
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
                    return ds.Tables[0].Rows[0].Field<long>("Total") > 0;
                }

                return false;
            }
            catch (MySqlException ex)
            {
                string msg = $"Error al actualizar de la impresora {requestDb.PrinterId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = $"Error al actualizar de la impresora {requestDb.PrinterId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public PrinterDetailDbObject GetPrinterDetail(int groupId, int printerId)
        {
            try
            {
                PrinterDetailDbObject response = new PrinterDetailDbObject();
                string procName = $"{ProcedurePrefix}_pr_PRINTER_DETAIL_GET";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter(GroupParam, MySqlDbType.VarChar) { Value = groupId });
                cmd.Parameters.Add(new MySqlParameter(PrinterParam, MySqlDbType.VarChar) { Value = printerId });

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    response = new PrinterDetailDbObject();
                    return response.Create(ds.Tables[0].Rows[0]);
                }

                return response;
            }
            catch (MySqlException ex)
            {
                string msg = $"Error al devolver el detalle de impresora {printerId} de en BBDD";
                Logger.LogError(ex, msg);
                return new PrinterDetailDbObject();
            }
            catch (Exception ex)
            {
                string msg = $"Error al devolver el detalle de impresora {printerId} de en BBDD";
                Logger.LogError(ex, msg);
                return new PrinterDetailDbObject();
            }
        }

        public List<PrinterTimesValuesDbObject> GetTimeVariation(int groupId, int printerId, out bool error)
        {
            try
            {
                error = false;
                List<PrinterTimesValuesDbObject> list = new List<PrinterTimesValuesDbObject>();
                string procName = $"{ProcedurePrefix}_pr_PRINT_LIST_TIMES";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter(GroupParam, MySqlDbType.Int32) { Value = groupId });
                cmd.Parameters.Add(new MySqlParameter(PrinterParam, MySqlDbType.Int32) { Value = printerId });

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    error = false;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        PrinterTimesValuesDbObject listResponse = new PrinterTimesValuesDbObject();
                        list.Add(listResponse.Create(row));
                    }
                }
                return list;
            }
            catch (MySqlException ex)
            {
                error = true;
                string msg = $"Error al devolver el listado de tiempos de impresiones de detalle de la impresora {printerId} en BBDD";
                Logger.LogError(ex, msg);
                return new List<PrinterTimesValuesDbObject>();
            }
            catch (Exception ex)
            {
                error = true;
                string msg = $"Error al devolver el listado de tiempos de impresiones de detalle de la impresora {printerId} en BBDD";
                Logger.LogError(ex, msg);
                return new List<PrinterTimesValuesDbObject>();
            }
        }

        public DeletedDbObject DeletePrinter(int printerId, int groupId, out int? error)
        {
            error = null;
            try
            {
                DeletedDbObject response = new DeletedDbObject { SuccesfullDelete = false };

                string procName = $"{ProcedurePrefix}_pr_PRINTER_DELETE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter(GroupParam, MySqlDbType.Int32) { Value = groupId });
                cmd.Parameters.Add(new MySqlParameter(PrinterParam, MySqlDbType.Int32) { Value = printerId });

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                var errorDb = Convert.ToInt32(errorParam.Value);
                if (errorDb != 0)
                {
                    error = errorDb;
                    response.Id = printerId;
                    return response;
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    response = response.Create(ds.Tables[0].Rows[0]);
                    response.SuccesfullDelete = true;
                    return response;
                }

                return response;
            }
            catch (MySqlException ex)
            {
                string msg = $"Error al eliminar la impresora {printerId} para el grupo {groupId} en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return new DeletedDbObject { SuccesfullDelete = false };
            }
            catch (Exception ex)
            {
                string msg = $"Error al eliminar la impresora {printerId} para el grupo {groupId} en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return new DeletedDbObject { SuccesfullDelete = false };
            }
        }

        public FileResponseDbObject GetPrinterImageData(int printerId, int groupId, out bool error)
        {
            error = false;
            FileResponseDbObject responseDb = new FileResponseDbObject();
            try
            {
                string procName = $"{ProcedurePrefix}_pr_PRINTER_GET_IMAGE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter(PrinterParam, MySqlDbType.Int32) { Value = printerId });
                cmd.Parameters.Add(new MySqlParameter(GroupParam, MySqlDbType.Int32) { Value = groupId });
                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    error = false;
                    return responseDb.Create(ds.Tables[0].Rows[0]);
                }

                return responseDb;
            }
            catch (MySqlException ex)
            {
                error = true;
                string msg = $"Error al guardar los datos de la imagen en la impresora {printerId} BBDD";
                Logger.LogError(ex, msg);
                return new FileResponseDbObject();
            }
            catch (Exception ex)
            {
                error = true;
                string msg = $"Error al guardar los datos de la imagen en la impresora {printerId} BBDD";
                Logger.LogError(ex, msg);
                return new FileResponseDbObject();
            }
        }

        public bool DeletePrinterImageData(int printerId, int groupId)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_PRINTER_DELETE_IMAGE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter(PrinterParam, MySqlDbType.Int32) { Value = printerId });
                cmd.Parameters.Add(new MySqlParameter(GroupParam, MySqlDbType.Int32) { Value = groupId });
                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
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
                string msg = $"Error al borrar los datos de la imagen en la impresora {printerId} BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = $"Error al borrar los datos de la imagen en la impresora {printerId} BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }
    }

}
