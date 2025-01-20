using Dapper;
using DiamondLegends.DAL.Factories.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.DAL.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IDbConnectionFactory _connection;

        public CountryRepository(IDbConnectionFactory connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Country>> GetAll()
        {
            using (var connection = _connection.Create())
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<Country>("SELECT * FROM Countries");
            }
        }

        public async Task<Country?> GetById(int id)
        {
            using (var connection = _connection.Create())
            {
                await connection.OpenAsync();
                return await connection.QuerySingleAsync<Country>(
                    "SELECT * FROM Countries WHERE Id = @Id",
                    new { Id = id }
                );
            }
        }
    }
}