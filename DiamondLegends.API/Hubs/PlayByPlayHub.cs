using DiamondLegends.BLL.Generators;
using DiamondLegends.BLL.Generators.Interfaces;
using DiamondLegends.BLL.Services;
using DiamondLegends.DAL.Repositories.Interfaces;
using DiamondLegends.Domain.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.ObjectModel;

namespace DiamondLegends.API.Hubs
{
    public class PlayByPlayHub : Hub
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ILineUpGenerator _lineUpGenerator;

        public PlayByPlayHub(IGameRepository gameRepository, IPlayerRepository playerRepository, ITeamRepository teamRepository, ILineUpGenerator lineUpGenerator)
        {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _lineUpGenerator = lineUpGenerator;
        }

        public async Task Test()
        {
            await Clients.Caller.SendAsync("Test");
        }

        public async Task SimulateGame(GameLineUpWithGameId gameLineUpWithGameId)
        {
            int id = gameLineUpWithGameId.GameId;
            GameLineUp lineUp = gameLineUpWithGameId;

            Game? game = await _gameRepository.GetById(id);

            if (game is null)
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

            foreach (GameLineUpDetail detail in lineUp.LineUpDetails)
            {
                if (detail.Order < 10)
                {
                    offensiveLineUp.Add(new GameOffensiveStats()
                    {
                        Game = game,
                        Player = await _playerRepository.GetById(detail.PlayerId),
                        Position = detail.Position,
                        Order = detail.Order,
                    });
                }
            }

            // Generate lineUp for opponent
            int opponentId = pitcher.Team == game.Home ? game.Home.Id : game.Away.Id;
            Team? opponent = await _teamRepository.GetById(opponentId);

            if (opponent is null)
            {
                throw new ArgumentNullException("L'equipe adverse n'existe pas.");
            }

            // TODO: Check if game is not already played

            List<GameOffensiveStats> opponentLineUp = await _lineUpGenerator.GenerateLineUp(opponent, game);
            GamePitchingStats opponentStartingPitcher = await _lineUpGenerator.GenerateStartingPitcher(opponent, game);

            GameSimulator simulation = new GameSimulator(game, offensiveLineUp, startingPitcher, opponentLineUp, opponentStartingPitcher, true, _gameRepository);

            ObservableCollection<GameEvent> gameEvents = simulation.GetEvents();

            gameEvents.CollectionChanged += async (sender, e) => await Clients.Caller.SendAsync("SendEvents", gameEvents.Last());

            game = await simulation.Simulate();

            await Clients.Caller.SendAsync("EndGame", game);
        }
    }
}
