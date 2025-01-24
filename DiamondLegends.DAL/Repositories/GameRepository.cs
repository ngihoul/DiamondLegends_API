using Dapper;
using DiamondLegends.DAL.Factories.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.DAL.Mappers;
using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;

namespace DiamondLegends.DAL.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly IDbConnectionFactory _connection;

        public GameRepository(IDbConnectionFactory connection)
        {
            _connection = connection;
        }

        public async Task<Game> Create(Game game)
        {
            using (var connection = _connection.Create())
            {
                await connection.OpenAsync();

                game.Id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO Games([Date], Season, Away, Home) " +
                    "OUTPUT INSERTED.Id " +
                    "VALUES (@Date, @Season, @Away, @Home)",
                    new { Date = game.Date, Season = game.Season, Away = game.Away.Id, Home = game.Home.Id }
                );

                return game;
            }
        }

        public async Task<List<Game>?> GetAll(GameQuery query)
        {
            using (var connection = _connection.Create())
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = "SELECT G.Id, G.Date, G.Season, T.Id AS AwayId, T.Name AS AwayName, T.Abbreviation AS AwayAbbreviation, T2.Id AS HomeId, T2.Name AS HomeName, T2.Abbreviation AS HomeAbbreviation, G.Away_runs, G.Home_runs, G.Away_hits, G.Home_hits, G.Away_errors, G.Home_errors " +
                    "FROM Games AS G " +
                    "JOIN Teams AS T ON G.Away = T.Id " +
                    "JOIN Teams AS T2 ON G.Home = T2.Id";

                List<string> where = new List<string>();

                if (query.LeagueId is not null)
                {
                    where.Add("T.League = @LeagueId");
                    command.Parameters.AddWithValue("@LeagueId", query.LeagueId);
                }

                if (query.TeamId is not null)
                {
                    where.Add("(T.Id = @TeamId OR T2.Id = @TeamId)");
                    command.Parameters.AddWithValue("@TeamId", query.TeamId);
                }

                if(query.Season is not null)
                {
                    where.Add("G.Season = @Season");
                    command.Parameters.AddWithValue("@Season", query.Season);
                }

                if(query.Month is not null)
                {
                    where.Add("MONTH(G.Date) = @Month");
                    command.Parameters.AddWithValue("@Month", query.Month);
                }

                if(query.Day is not null)
                {
                    where.Add("DAY(G.Date) = @Day");
                    command.Parameters.AddWithValue("@Day", query.Day);
                }

                if(where.Count > 0)
                {
                    command.CommandText += " WHERE " + string.Join(" AND ", where);
                }

                await connection.OpenAsync();

                List<Game> games = new List<Game>();

                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {
                    games.Add(GameMappers.FullGame(reader));
                }

                return games;
            }
        }
    }
}
