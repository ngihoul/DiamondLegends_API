using DiamondLegends.BLL.Generators.Interfaces;
using DiamondLegends.BLL.Services.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGameGenerator _gameGenerator;

        public GameService(IGameRepository gameRepository, IGameGenerator gameGenerator)
        {
            _gameRepository = gameRepository;
            _gameGenerator = gameGenerator;
        }

        public async Task<Game> GetById(int id)
        {
            Game? game = await _gameRepository.GetById(id);

            if (game is null)
            {
                throw new ArgumentNullException("Le match n'existe pas.");
            }

            return game;
        }

        public async Task<List<Game>> GetAll(GameQuery? query = null)
        {
            // QUESTION : a faire en BLL ou API ?
            if (query is not null)
            {
                if (query.LeagueId is not null && query.LeagueId <= 0)
                {
                    throw new ArgumentNullException("L'id de la ligue n'est pas valable");
                }

                if (query.TeamId is not null && query.TeamId <= 0)
                {
                    throw new ArgumentNullException("L'id de l'equipe n'est pas valable");
                }

                if (query.Month is not null && query.Month <= 0 || query.Month > 12)
                {
                    throw new ArgumentNullException("Le mois n'est pas valable");
                }

                if (query.Season is not null && query.Season <= 0)
                {
                    throw new ArgumentNullException("L'annee n'est pas valable");
                }

                if (query.Day is not null && query.Day <= 0 || query.Day > 31)
                {
                    throw new ArgumentNullException("Le jour n'est pas valable");
                }
            }

            List<Game>? games = await _gameRepository.GetAll(query);

            return games;
        }

        public async Task<Game> Play(int id, List<GameOffensiveStats> offensiveLineUp, GamePitchingStats startingPitcher)
        {
            Game? game = await _gameRepository.GetById(id);

            if(game is null)
            {
                throw new ArgumentNullException("Le match n'existe pas.");
            }

            // Generate lineUp for opponent
            Team opponent = startingPitcher.Player.Team == game.Home ? game.Home : game.Away;
            List<GameOffensiveStats> opponentLineUp = _gameGenerator.GenerateLineUp(opponent, game);
            GamePitchingStats opponentStartingPitcher = _gameGenerator.GenerateStartingPitcher(opponent);

            // Simulate Game
            game = await _gameGenerator.SimulateGame(game, offensiveLineUp, startingPitcher, opponentLineUp, opponentStartingPitcher);

            // return Game with stats
            return game;
        }
    }
}
