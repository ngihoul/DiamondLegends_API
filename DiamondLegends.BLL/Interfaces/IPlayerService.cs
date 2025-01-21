using DiamondLegends.Domain.Models;

namespace DiamondLegends.BLL.Interfaces
{
    public interface IPlayerService
    {
        public Task<Player> GetById(int id);
    }
}
