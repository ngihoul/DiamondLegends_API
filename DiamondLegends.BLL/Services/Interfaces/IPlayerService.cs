using DiamondLegends.Domain.Models;

namespace DiamondLegends.BLL.Services.Interfaces
{
    public interface IPlayerService
    {
        public Task<Player> GetById(int id);
    }
}
