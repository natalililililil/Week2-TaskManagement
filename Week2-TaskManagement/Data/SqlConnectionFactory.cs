using Microsoft.Data.SqlClient;
using System.Data;

namespace Week2_TaskManagement.Data
{
    internal class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException("Отсутвует строка подключения");
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
