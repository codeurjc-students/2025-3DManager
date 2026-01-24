using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DMANAGER_APP.DAL.Base
{
    public class MySQLManager
    {
        public readonly MySQLDataSource _dataSource;
        public readonly ILogger _logger;

        public string _3DMANAGER_ProcedurePrefix => MySQLDataSource._3DMANAGER_ProcedurePrefix;
        public string ProcedurePrefix => _dataSource.ProcedurePrefix;
        public MySqlConnection Connection => _dataSource.GetDataSource();
        public ILogger Logger => _logger;
        public MySQLManager(IDataSource<MySqlConnection> dataSourceFactory, ILogger logger)
        {
            _dataSource = (MySQLDataSource)dataSourceFactory;
            _logger = logger;
        }

        public MySqlParameter CreateReturnValueParameter(string parameterName, MySqlDbType type)
        {
            return new MySqlParameter(parameterName, type)
            {
                Direction = ParameterDirection.Output
            };
        }
    }
}
