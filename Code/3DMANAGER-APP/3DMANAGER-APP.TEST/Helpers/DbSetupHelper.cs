using MySql.Data.MySqlClient;

namespace _3DMANAGER_APP.TEST.Helpers
{
    public static class DbSetupHelper
    {
        public static async Task ExecuteAsync(string connectionString, string sql)
        {
            await using var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            await using var cmd = new MySqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
