using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.BLL.Services.Interfaces
{
    public interface IGameService
    {
        public Task<Game> GetById(int id);
        public Task<List<Game>> GetAll(GameQuery? query = null);
        public Task<Game> Play(int id, List<GameOffensiveStats> offensiveLineUp, GamePitchingStats startingPitcher);
    }
}
