using Dapper;
using DiamondLegends.DAL.Factories.Interfaces;
using DiamondLegends.DAL.Mappers;
using DiamondLegends.DAL.Repositories.Interfaces;
using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;

namespace DiamondLegends.DAL.Repositories
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly IDbConnectionFactory _connection;
        private readonly ITeamRepository _teamRepository;

        public LeagueRepository(IDbConnectionFactory connection, ITeamRepository teamRepository)
        {
            _connection = connection;
            _teamRepository = teamRepository;
        }

        public async Task<League> Create(League league)
        {
            using (var connection = _connection.Create())
            {
                await connection.OpenAsync();

                league.Id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO Leagues(Name, In_game_date) " +
                    "OUTPUT INSERTED.Id " +
                    "VALUES (@Name, @In_game_date)",
                    new { Name = league.Name, In_game_date = league.InGameDate }
                );

                return league;
            }
        }

        public async Task<League?> GetById(int id)
        {
            using (var connection = _connection.Create())
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = "SELECT * FROM Leagues AS L " +
                    "JOIN Teams AS T ON L.Id = T.League " +
                    "WHERE L.Id = @Id";

                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();

                League league = null;

                using SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    List<Team>? teams = await _teamRepository.GetAllByLeague((int)reader["Id"]);
                    league = LeagueMappers.FullLeague(reader, teams);
                }

                return league;
            }
        }

        public async Task<League?> UpdateInGameDate(League league)
        {
            int rowAffected = 0;

            using (var connection = _connection.Create())
            {
                await connection.OpenAsync();

                var sql1 = "UPDATE Leagues SET In_game_date = @InGameDate WHERE Id = @leagueId";
                rowAffected = await connection.ExecuteAsync(sql1, new { leagueId = league.Id, InGameDate = league.InGameDate });
            }

            return rowAffected > 0 ? league : null;
        }
    }
}
