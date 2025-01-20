using Dapper;
using DiamondLegends.DAL.Factories.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;

namespace DiamondLegends.DAL.Repositories
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly IDbConnectionFactory _connection;

        public LeagueRepository(IDbConnectionFactory connection)
        {
            _connection = connection;
        }

        public async Task<League> Create(League league)
        {
            using(var connection = _connection.Create())
            {
                await connection.OpenAsync();

                league.Id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO Leagues(Name) " +
                    "OUTPUT INSERTED.Id " +
                    "VALUES (@Name)",
                    league
                );

                return league;
            }
        }
    }
}
