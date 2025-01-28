using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.BLL.Generators.Interfaces
{
    public interface IGameGenerator
    {
        public List<GameOffensiveStats> GenerateLineUp(Team team, Game game);
        public GamePitchingStats GenerateStartingPitcher(Team team, Game game);
        public Task<Game> SimulateGame(Game game, List<GameOffensiveStats> offensiveLineUp, GamePitchingStats startingPitcher, List<GameOffensiveStats> opponentLineUp, GamePitchingStats opponentStartingPitcher);
    }
}
