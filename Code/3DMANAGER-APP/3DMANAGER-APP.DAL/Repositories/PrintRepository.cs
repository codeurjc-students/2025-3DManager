using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.File;
using _3DMANAGER_APP.DAL.Models.Print;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;

namespace _3DMANAGER_APP.DAL.Repositories
{
    public class PrintRepository : MySQLManager, IPrintRepository
    {
        private const string ErrorConstant = "CodigoError";
        private const string GroupParam = "P_CD_GROUP";
        public PrintRepository(IDataSource<MySqlConnection> dataSourceFactory, ILogger<PrintRepository> logger)
            : base(dataSourceFactory, logger)
        {
        }

        public List<PrintListResponseDbObject> GetPrintList(int group, int pageNumber, int pageSize, out int totalItems, out bool error)
        {
            error = false;
            totalItems = 0;
            try
            {
                List<PrintListResponseDbObject> list = new List<PrintListResponseDbObject>();
                string procName = $"{ProcedurePrefix}_pr_PRINT_LIST";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter(GroupParam, MySqlDbType.Int32) { Value = group });
                cmd.Parameters.Add(new MySqlParameter("P_PAGE_NUMBER", MySqlDbType.Int32) { Value = pageNumber });
                cmd.Parameters.Add(new MySqlParameter("P_PAGE_SIZE", MySqlDbType.Int32) { Value = pageSize });

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
                error = true;
                string msg = $"Error al devolver el listado de impresiones del grupo {group} en BBDD";
                Logger.LogError(ex, msg);
                return new List<PrintListResponseDbObject>();
            }
            catch (Exception ex)
            {
                error = true;
                string msg = $"Error al devolver el listado de impresiones del grupo {group} en BBDD";
                Logger.LogError(ex, msg);
                return new List<PrintListResponseDbObject>();
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
                cmd.Parameters.Add(new MySqlParameter("P_PRINT_PROGRESS", MySqlDbType.Int32) { Value = request.PrintProgress });

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
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
                string msg = $"Error al guardar los datos de la imagen de la impresión {printId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = $"Error al guardar los datos de la imagen de la impresión {printId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public List<PrintListResponseDbObject> GetPrintListByType(int group, int pageNumber, int pageSize, int type, int id, out int totalItems, out bool error)
        {
            totalItems = 0;
            error = false;
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

                cmd.Parameters.Add(new MySqlParameter(GroupParam, MySqlDbType.Int32) { Value = group });
                cmd.Parameters.Add(new MySqlParameter("P_PAGE_NUMBER", MySqlDbType.Int32) { Value = pageNumber });
                cmd.Parameters.Add(new MySqlParameter("P_PAGE_SIZE", MySqlDbType.Int32) { Value = pageSize });
                cmd.Parameters.Add(new MySqlParameter("P_ID", MySqlDbType.Int32) { Value = id });

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    error = false;
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
                error = true;
                string msg = $"Error al devolver el listado de impresiones de detalle del grupo {group} para el tipo {type} en BBDD";
                Logger.LogError(ex, msg);
                return new List<PrintListResponseDbObject>();
            }
            catch (Exception ex)
            {
                error = true;
                string msg = $"Error al devolver el listado de impresiones de detalle del grupo {group} para el tipo {type} en BBDD";
                Logger.LogError(ex, msg);
                return new List<PrintListResponseDbObject>();
            }
        }

        public bool UpdatePrint(PrintDetailRequestDbObject request)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_PRINT_UPDATE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter(GroupParam, MySqlDbType.Int32) { Value = request.GroupId });
                cmd.Parameters.Add(new MySqlParameter("P_CD_PRINT", MySqlDbType.Int32) { Value = request.PrintId });
                cmd.Parameters.Add(new MySqlParameter("P_DS_NAME", MySqlDbType.VarChar) { Value = request.PrintName });
                cmd.Parameters.Add(new MySqlParameter("P_DS_DESCRIPTION", MySqlDbType.VarChar) { Value = request.PrintDescription });
                cmd.Parameters.Add(new MySqlParameter("P_NM_REAL_TIME", MySqlDbType.Int32) { Value = request.PrintRealTime });

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
                string msg = $"Error al actualizar la impresión {request.PrintId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = $"Error al actualizar la impresión {request.PrintId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public PrintDetailDbObject GetPrintDetail(int groupId, int printId)
        {
            try
            {
                PrintDetailDbObject response = new PrintDetailDbObject();
                string procName = $"{ProcedurePrefix}_pr_PRINT_DETAIL_GET";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter(GroupParam, MySqlDbType.VarChar) { Value = groupId });
                cmd.Parameters.Add(new MySqlParameter("P_CD_PRINT", MySqlDbType.VarChar) { Value = printId });

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    response = new PrintDetailDbObject();
                    return response.Create(ds.Tables[0].Rows[0]);
                }

                return response;
            }
            catch (MySqlException ex)
            {
                string msg = $"Error al devolver el detalle de impresión {printId} de en BBDD";
                Logger.LogError(ex, msg);
                return new PrintDetailDbObject();
            }
            catch (Exception ex)
            {
                string msg = $"Error al devolver el detalle de impresión {printId} de en BBDD";
                Logger.LogError(ex, msg);
                return new PrintDetailDbObject();
            }
        }

        public List<PrintCommentDbObject> GetPrintComments(int groupId, int printId, out bool error)
        {
            try
            {
                error = false;
                List<PrintCommentDbObject> list = new List<PrintCommentDbObject>();
                string procName = $"{ProcedurePrefix}_pr_PRINT_COMMENTS_LIST";

                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter(GroupParam, MySqlDbType.Int32) { Value = groupId });
                cmd.Parameters.Add(new MySqlParameter("P_PRINT_ID", MySqlDbType.Int32) { Value = printId });

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    error = false;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        PrintCommentDbObject comment = new PrintCommentDbObject();
                        list.Add(comment.Create(row));
                    }
                }

                return list;
            }
            catch (MySqlException ex)
            {
                error = true;
                string msg = $"Error al obtener los comentarios de impresión {printId} en BBDD";
                Logger.LogError(ex, msg);
                return new List<PrintCommentDbObject>();
            }
            catch (Exception ex)
            {
                error = true;
                string msg = $"Error al obtener los comentarios de impresión {printId} en BBDD";
                Logger.LogError(ex, msg);
                return new List<PrintCommentDbObject>();
            }
        }

        public int PostPrintComment(PrintCommentRequestDbObject request)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_PRINT_COMMENT_POST";

                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_COMMENT", MySqlDbType.VarChar) { Value = request.Comment });
                cmd.Parameters.Add(new MySqlParameter("P_USER_ID", MySqlDbType.Int32) { Value = request.UserId });
                cmd.Parameters.Add(new MySqlParameter("P_PRINT_ID", MySqlDbType.Int32) { Value = request.PrintId });

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                int errorCode = Convert.ToInt32(errorParam.Value);
                if (errorCode != 0)
                {
                    string msg = $"Error al insertar el comentario del usuario {request.UserId} en la impresion {request.PrintId} . Código: {errorCode}";
                    Logger.LogError(msg);
                    return 0;
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0]["COMMENT_ID"]);
                }
                return 0;
            }
            catch (MySqlException ex)
            {
                string msg = $"Error al insertar el comentario del usuario {request.UserId} en la impresion {request.PrintId} en BBDD";
                Logger.LogError(ex, msg);
                return 0;
            }
            catch (Exception ex)
            {
                string msg = $"Error al insertar el comentario del usuario {request.UserId} en la impresion {request.PrintId} en BBDD";
                Logger.LogError(ex, msg);
                return 0;
            }
        }

        public DeletedDbObject DeletePrint(int printId, int groupId, out int? error)
        {
            error = null;
            try
            {
                DeletedDbObject response = new DeletedDbObject { SuccesfullDelete = false };

                string procName = $"{ProcedurePrefix}_pr_PRINT_DELETE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter(GroupParam, MySqlDbType.Int32) { Value = groupId });
                cmd.Parameters.Add(new MySqlParameter("P_CD_PRINT", MySqlDbType.Int32) { Value = printId });

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                var errorDb = Convert.ToInt32(errorParam.Value);
                if (errorDb != 0)
                {
                    error = errorDb;
                    response.Id = printId;
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
                string msg = $"Error al eliminar la impresión {printId} en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return new DeletedDbObject { SuccesfullDelete = false };
            }
            catch (Exception ex)
            {
                string msg = $"Error al eliminar la impresión {printId} en BBDD";
                Logger.LogError(ex, msg);
                error = 500;
                return new DeletedDbObject { SuccesfullDelete = false };
            }
        }

        public FileResponseDbObject GetPrintImageData(int printId, int groupId, out bool error)
        {
            error = false;
            FileResponseDbObject responseDb = new FileResponseDbObject();
            try
            {
                string procName = $"{ProcedurePrefix}_pr_PRINT_GET_IMAGE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_PRINT", MySqlDbType.Int32) { Value = printId });
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
                string msg = $"Error al guardar los datos de fichero STL en la impresión {printId} BBDD";
                Logger.LogError(ex, msg);
                return new FileResponseDbObject();
            }
            catch (Exception ex)
            {
                error = true;
                string msg = $"Error al guardar los datos de fichero STL en la impresión {printId} BBDD";
                Logger.LogError(ex, msg);
                return new FileResponseDbObject();
            }
        }

        public bool DeletePrintImageData(int printId, int groupId)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_PRINT_DELETE_IMAGE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_PRINT", MySqlDbType.Int32) { Value = printId });
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
                string msg = $"Error al borrar los datos de fichero STL en la impresión {printId} BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = $"Error al borrar los datos de fichero STL en la impresión {printId} BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }

        public bool DeletePrintComment(int commentId)
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_COMMENT_DELETE";
                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new MySqlParameter("P_CD_COMMENT", MySqlDbType.Int32) { Value = commentId });
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
                string msg = $"Error al borrar el comentario {commentId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
            catch (Exception ex)
            {
                string msg = $"Error al borrar el comentario {commentId} en BBDD";
                Logger.LogError(ex, msg);
                return false;
            }
        }
    }
}
