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
    public class LeagueRepository : ILeagueRepository
    {
        private readonly SqlConnection _connection;

        public LeagueRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<League> Create(League league)
        {
            await _connection.OpenAsync();

            league.Id = await _connection.QuerySingleAsync<int>(
                "INSERT INTO Leagues(Name) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@Name)",
                league
            );

            await _connection.CloseAsync();

            return league;
        }
    }
}
