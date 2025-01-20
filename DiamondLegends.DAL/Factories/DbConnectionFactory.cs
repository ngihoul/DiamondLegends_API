using DiamondLegends.DAL.Factories.Interfaces;
using Microsoft.Data.SqlClient;

namespace DiamondLegends.DAL.Factories
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection Create()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
