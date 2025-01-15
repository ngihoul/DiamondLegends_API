using Dapper;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.DAL.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly SqlConnection _connection;

        public CountryRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<Country> GetById(int id)
        {
            await _connection.OpenAsync();

            Country country = _connection.QuerySingle<Country>(
                "SELECT * FROM Countries WHERE Id = @Id",
                new { Id = id }
            );

            await _connection.CloseAsync();

            return country;
        }
    }
}
