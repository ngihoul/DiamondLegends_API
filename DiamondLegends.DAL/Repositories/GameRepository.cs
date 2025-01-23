using Dapper;
using DiamondLegends.DAL.Factories.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;

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
    }
}
