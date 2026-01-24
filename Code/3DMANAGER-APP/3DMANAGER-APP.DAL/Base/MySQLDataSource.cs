using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DMANAGER_APP.DAL.Base
{
    public class MySQLDataSource : IDataSource<MySqlConnection>
    {
        public static string _3DMANAGER_ProcedurePrefix = "3DMANAGER";
        private readonly MySqlConnection _connection;
        private readonly string _procedurePrefix;

        public string ProcedurePrefix => _procedurePrefix;

        public MySQLDataSource(string connectionString, string procedurePrefix)
        {
            _connection = new MySqlConnection(connectionString);
            _procedurePrefix = procedurePrefix;
        }

        public MySqlConnection GetDataSource()
        {
            return _connection;
        }
    }
}
