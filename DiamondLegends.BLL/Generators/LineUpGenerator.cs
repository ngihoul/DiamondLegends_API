using DiamondLegends.BLL.Generators.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Enums;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.BLL.Generators
{
    public class LineUpGenerator(IPlayerRepository _playerRepository) : ILineUpGenerator
    {
        public async Task<List<GameOffensiveStats>> GenerateLineUp(Team team, Game game)
        {
            List<GameOffensiveStats> lineUp = new List<GameOffensiveStats>();

            // QUESTION : best to initialize to null ?
            PlayerWithBestPosition? bestCatcher = new PlayerWithBestPosition();
            PlayerWithBestPosition? bestFirstBase = new PlayerWithBestPosition();
            PlayerWithBestPosition? bestSecondBase = new PlayerWithBestPosition();
            PlayerWithBestPosition? bestThirdBase = new PlayerWithBestPosition();
            PlayerWithBestPosition? bestShortStop = new PlayerWithBestPosition();
            PlayerWithBestPosition? bestLeftField = new PlayerWithBestPosition();
            PlayerWithBestPosition? bestCenterField = new PlayerWithBestPosition();
            PlayerWithBestPosition? bestRightField = new PlayerWithBestPosition();
            PlayerWithBestPosition? bestDesignatedHitter = new PlayerWithBestPosition();

            // Add to test if player is already selected for a position // Cannot be twice in the lineup
            List<Player> playerList = new List<Player>();

            // Find who is best at each positions
            foreach (Player player in team.Players!)
            {
                Player playerWithSkills = await _playerRepository.GetById(player.Id);

                bestCatcher = GetBestPlayer(playerWithSkills, bestCatcher, Position.Catcher, playerList);
                bestFirstBase = GetBestPlayer(playerWithSkills, bestFirstBase, Position.FirstBase, playerList);
                bestSecondBase = GetBestPlayer(playerWithSkills, bestSecondBase, Position.SecondBase, playerList);
                bestThirdBase = GetBestPlayer(playerWithSkills, bestThirdBase, Position.ThirdBase, playerList);
                bestShortStop = GetBestPlayer(playerWithSkills, bestShortStop, Position.ShortStop, playerList);
                bestLeftField = GetBestPlayer(playerWithSkills, bestLeftField, Position.LeftField, playerList);
                bestCenterField = GetBestPlayer(playerWithSkills, bestCenterField, Position.CenterField, playerList);
                bestRightField = GetBestPlayer(playerWithSkills, bestRightField, Position.RightField, playerList);
                bestDesignatedHitter = GetBestPlayer(playerWithSkills, bestDesignatedHitter, Position.DesignatedHitter, playerList);
            }

            List<PlayerWithBestPosition> bestFielders = new List<PlayerWithBestPosition>() { bestCatcher, bestFirstBase, bestSecondBase, bestThirdBase, bestShortStop, bestLeftField, bestCenterField, bestRightField, bestDesignatedHitter };

            // Sort players based on player.AverageBattingSkills
            bestFielders = bestFielders.OrderByDescending(player => player.Player.AverageBattingSkill()).ToList();

            // generate lineup based on BattingSkill
            for (int i = 0; i < bestFielders.Count; i++)
            {
                lineUp.Add(
                    new GameOffensiveStats()
                    {
                        Game = game,
                        Player = bestFielders[i].Player,
                        Position = bestFielders[i].Position,
                        Order = i
                    }
                );
            }

            return lineUp;
        }

        public async Task<GamePitchingStats> GenerateStartingPitcher(Team team, Game game)
        {
            List<Player> startingPitchers = team.Players.Where(p => p.Positions.Contains(Position.StartingPitcher)).ToList();

            Player bestStartingPitcher = null;

            // TODO : test if OK to test only on Energy
            foreach (Player pitcher in startingPitchers)
            {
                Player pitcherToTest = await _playerRepository.GetById(pitcher.Id);
                if (bestStartingPitcher is null || bestStartingPitcher.Energy < pitcherToTest.Energy)
                {
                    bestStartingPitcher = pitcherToTest;
                }
            }

            return new GamePitchingStats()
            {
                Game = game,
                Player = bestStartingPitcher
            };
        }

        private PlayerWithBestPosition GetBestPlayer(Player playerToTest, PlayerWithBestPosition best, Position position, List<Player> playerList)
        {
            if (playerToTest.Positions.Contains(position))
            {
                if ((best is null || best.Player is null || best.Player.AverageFielderSkill() < playerToTest.AverageFielderSkill()) && !playerList.Contains(playerToTest))
                {
                    playerList.Add(playerToTest);

                    best.Player = playerToTest;
                    best.Position = position;
                }
            }

            return best!;
        }
    }

    // Intermediate class to find the best position of a specific player among a List of positions
    public class PlayerWithBestPosition
    {
        public Player Player { get; set; }
        public Position Position { get; set; }
    }
}
