using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.BLL.Generators.Interfaces
{
    public interface ILineUpGenerator
    {
        public Task<List<GameOffensiveStats>> GenerateLineUp(Team team, Game game);
        public Task<GamePitchingStats> GenerateStartingPitcher(Team team, Game game);
    }
}
