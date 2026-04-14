using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;

namespace _3DMANAGER_APP.DAL.Repositories
{
    public class DailyTaskRepository : MySQLManager, IDailyTaskRepository
    {
        private const string ErrorConstant = "CodigoError";

        public DailyTaskRepository(IDataSource<MySqlConnection> dataSourceFactory, ILogger<DailyTaskRepository> logger) : base(dataSourceFactory, logger)
        {
        }

        public async Task CleanOldDataAsync()
        {
            try
            {
                string procName = $"{ProcedurePrefix}_pr_CLEAN_OLD_DATA";

                using var cmd = new MySqlCommand(procName, Connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                var errorParam = CreateReturnValueParameter(ErrorConstant, MySqlDbType.Int32);
                cmd.Parameters.Add(errorParam);

                using var adapter = new MySqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                var errorDb = Convert.ToInt32(errorParam.Value);
                if (errorDb != 0)
                {
                    Logger.LogError($"Error al limpiar datos antiguos. Código: {errorDb}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error al ejecutar CleanOldDataAsync");
            }
        }
    }

}
