using DiamondLegends.BLL.Generators;
using DiamondLegends.BLL.Generators.Interfaces;
using DiamondLegends.BLL.Services.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ILineUpGenerator _lineUpGenerator;

        public GameService(IGameRepository gameRepository, IPlayerRepository playerRepository, ITeamRepository teamRepository, ILineUpGenerator lineUpGenerator)
        {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _lineUpGenerator = lineUpGenerator;
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

        public async Task<Game> Play(int id, GameLineUp lineUp)
        {
            Game? game = await _gameRepository.GetById(id);

            if(game is null)
            {
                throw new ArgumentNullException("Le match n'existe pas.");
            }

            // Generate lineUp
            // Starting Pitcher
            int startingPitcherId = lineUp.LineUpDetails.Where(d => d.Position == Domain.Enums.Position.StartingPitcher).First().PlayerId;

            Player? pitcher = await _playerRepository.GetById(startingPitcherId);

            if (pitcher is null)
            {
                throw new ArgumentNullException("Le lanceur partant n'existe pas");
            }

            GamePitchingStats startingPitcher = new GamePitchingStats()
            {
                Game = game,
                Player = pitcher
            };

            // Offensive lineup
            List<GameOffensiveStats> offensiveLineUp = new List<GameOffensiveStats>();

            foreach (GameLineUpDetails details in lineUp.LineUpDetails) { 
                if(details.Order < 10)
                {
                    offensiveLineUp.Add(new GameOffensiveStats()
                    {
                        Game = game,
                        Player = await _playerRepository.GetById(details.PlayerId),
                        Position = details.Position,
                        Order = details.Order,
                    });
                }
                
            }

            // Generate lineUp for opponent
            int opponentId = pitcher.Team == game.Home ? game.Home.Id : game.Away.Id;
            Team? opponent = await _teamRepository.GetById(opponentId);

            if(opponent is null)
            {
                throw new ArgumentNullException("L'equipe adverse n'existe pas.");
            }

            // TODO: Check if game is not already played

            List<GameOffensiveStats> opponentLineUp = await _lineUpGenerator.GenerateLineUp(opponent, game);
            GamePitchingStats opponentStartingPitcher = await _lineUpGenerator.GenerateStartingPitcher(opponent, game);

            // Simulate Game
            GameSimulator simulation = new GameSimulator(game, offensiveLineUp, startingPitcher, opponentLineUp, opponentStartingPitcher, _gameRepository);
            game = await simulation.Simulate();

            // return Game with stats
            return game;
        }
    }
}
