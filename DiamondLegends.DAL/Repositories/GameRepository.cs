using Dapper;
using DiamondLegends.DAL.Factories.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.DAL.Mappers;
using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DiamondLegends.DAL.Repositories
{
    public class GameRepository : IGameRepository
    {
        private const string BASE_SELECT_QUERY = "SELECT G.Id, G.Date, G.Season, G.Status, T.Id AS AwayId, T.Name AS AwayName, T.Abbreviation AS AwayAbbreviation, T2.Id AS HomeId, T2.Name AS HomeName, T2.Abbreviation AS HomeAbbreviation, G.Half_innings, G.Away_runs, G.Home_runs, G.Away_hits, G.Home_hits, G.Away_errors, G.Home_errors FROM Games AS G ";

        private readonly IDbConnectionFactory _connection;
        private readonly IOffensiveStatsRepository _offensiveStatsRepository;
        private readonly IPitchingStatsRepository _pitchingStatsRepository;

        public GameRepository(IDbConnectionFactory connection, IOffensiveStatsRepository offensiveStatsRepository, IPitchingStatsRepository pitchingStatsRepository)
        {
            _connection = connection;
            _offensiveStatsRepository = offensiveStatsRepository;
            _pitchingStatsRepository = pitchingStatsRepository;
        }

        public async Task<Game> Create(Game game)
        {
            using (var connection = _connection.Create())
            {
                await connection.OpenAsync();

                game.Id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO Games([Date], Season, Status, Away, Home) " +
                    "OUTPUT INSERTED.Id " +
                    "VALUES (@Date, @Season, @Away, @Home)",
                    new { Date = game.Date, Season = game.Season, Status = game.Status, Away = game.Away.Id, Home = game.Home.Id }
                );

                return game;
            }
        }

        public async Task<Game?> GetById(int id)
        {
            using (var connection = _connection.Create())
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = BASE_SELECT_QUERY +
                    "JOIN Teams AS T ON G.Away = T.Id " +
                    "JOIN Teams AS T2 ON G.Home = T2.Id " +
                    "WHERE G.Id = @Id";

                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();

                SqlDataReader reader = await command.ExecuteReaderAsync();

                Game? game = null;

                if (await reader.ReadAsync())
                {
                    game = GameMappers.FullGame(reader);
                }

                return game;
            }
        }

        public async Task<List<Game>?> GetAll(GameQuery query)
        {
            using (var connection = _connection.Create())
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = BASE_SELECT_QUERY +
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

                if (query.Season is not null)
                {
                    where.Add("G.Season = @Season");
                    command.Parameters.AddWithValue("@Season", query.Season);
                }

                if (query.Month is not null)
                {
                    where.Add("MONTH(G.Date) = @Month");
                    command.Parameters.AddWithValue("@Month", query.Month);
                }

                if (query.Day is not null)
                {
                    where.Add("DAY(G.Date) = @Day");
                    command.Parameters.AddWithValue("@Day", query.Day);
                }

                if (where.Count > 0)
                {
                    command.CommandText += " WHERE " + string.Join(" AND ", where);
                }

                await connection.OpenAsync();

                List<Game> games = new List<Game>();

                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    games.Add(GameMappers.FullGame(reader));
                }

                return games;
            }
        }

        public async Task<Game> Update(Game game)
        {
            using (var connection = _connection.Create())
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        using SqlCommand command = connection.CreateCommand();
                        command.Transaction = (SqlTransaction)transaction;

                        command.CommandText = "UPDATE [Games] SET Date = @Date, Season = @Season, Status = @Status, Away = @Away, Home = @Home, Half_innings = @HalfInnings, Away_runs = @AwayRuns, Home_runs = @HomeRuns, Away_hits = @AwayHits, Home_hits = @HomeHits, Away_errors = @AwayErrors, Home_errors = @HomeErrors WHERE Id = @Id";

                        command.Parameters.AddWithValue("@Date", game.Date);
                        command.Parameters.AddWithValue("@Season", game.Season);
                        command.Parameters.AddWithValue("@Status", game.Status);
                        command.Parameters.AddWithValue("@Away", game.Away.Id);
                        command.Parameters.AddWithValue("@Home", game.Home.Id);
                        command.Parameters.AddWithValue("@HalfInnings", game.HalfInnings);
                        command.Parameters.AddWithValue("@AwayRuns", game.AwayRuns);
                        command.Parameters.AddWithValue("@HomeRuns", game.HomeRuns);
                        command.Parameters.AddWithValue("@AwayHits", game.AwayHits);
                        command.Parameters.AddWithValue("@HomeHits", game.HomeHits);
                        command.Parameters.AddWithValue("@AwayErrors", game.AwayErrors);
                        command.Parameters.AddWithValue("@HomeErrors", game.HomeErrors);
                        command.Parameters.AddWithValue("@Id", game.Id);

                        int row = await command.ExecuteNonQueryAsync();
                        if (row == 0)
                        {
                            throw new Exception("Erreur lors de la mise à jour du match.");
                        }

                        foreach (GameOffensiveStats stats in game.OffensiveStats)
                        {
                            await _offensiveStatsRepository.Create(stats, connection, (SqlTransaction)transaction);
                        }

                        foreach (GamePitchingStats stats in game.PitchingStats)
                        {
                            await _pitchingStatsRepository.Create(stats, connection, (SqlTransaction)transaction);
                        }

                        await transaction.CommitAsync();
                        return game;
                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception("Erreur lors de la mise à jour du match", e);
                    }
                }
            }
        }
    }
}
