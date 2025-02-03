using DiamondLegends.Domain.Models;

namespace DiamondLegends.BLL.Services.Interfaces
{
    public interface IGameService
    {
        public Task<Game> GetById(int id);
        public Task<List<Game>> GetAll(GameQuery? query = null);
        public Task<Game> Play(int id, GameLineUp lineUp, bool playByPlay = false);
    }
}
