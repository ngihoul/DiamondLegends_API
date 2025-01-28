using DiamondLegends.BLL.Generators.Interfaces;
using DiamondLegends.Domain.Enums;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.BLL.Generators
{
    public class GameGenerator : IGameGenerator
    {
        public List<GameOffensiveStats> GenerateLineUp(Team team, Game game)
        {
            List<GameOffensiveStats> lineUp = new List<GameOffensiveStats>();

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
                bestCatcher = GetBestPlayer(player, bestCatcher, Position.Catcher, playerList);
                bestFirstBase = GetBestPlayer(player, bestFirstBase, Position.FirstBase, playerList);
                bestSecondBase = GetBestPlayer(player, bestSecondBase, Position.SecondBase, playerList);
                bestThirdBase = GetBestPlayer(player, bestThirdBase, Position.ThirdBase, playerList);
                bestShortStop = GetBestPlayer(player, bestShortStop, Position.ShortStop, playerList);
                bestLeftField = GetBestPlayer(player, bestLeftField, Position.LeftField, playerList);
                bestCenterField = GetBestPlayer(player, bestCenterField, Position.CenterField, playerList);
                bestRightField = GetBestPlayer(player, bestRightField, Position.RightField, playerList);
                bestDesignatedHitter = GetBestPlayer(player, bestDesignatedHitter, Position.DesignatedHitter, playerList);
            }

            List<PlayerWithBestPosition> bestFielders = new List<PlayerWithBestPosition>() { bestCatcher, bestFirstBase, bestSecondBase, bestThirdBase, bestShortStop, bestLeftField, bestCenterField, bestRightField, bestDesignatedHitter };

            // Sort players based on player.AverageBattingSkills
            bestFielders.OrderByDescending(player => player.Player.AverageBattingSkill());

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

        public GamePitchingStats GenerateStartingPitcher(Team team, Game game)
        {
            throw new NotImplementedException();
        }

        public Task<Game> SimulateGame(Game game, List<GameOffensiveStats> offensiveLineUp, GamePitchingStats startingPitcher, List<GameOffensiveStats> opponentLineUp, GamePitchingStats opponentStartingPitcher)
        {
            throw new NotImplementedException();
        }

        private PlayerWithBestPosition GetBestPlayer(Player playerToTest, PlayerWithBestPosition best, Position position, List<Player> playerList)
        {
            if (playerToTest.Positions.Contains(position))
            {
                if ((best is null || best.Player.AverageFielderSkill() < playerToTest.AverageFielderSkill()) && !playerList.Contains(playerToTest))
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


