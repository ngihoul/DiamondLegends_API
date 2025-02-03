using DiamondLegends.Domain.Models;

namespace DiamondLegends.DAL.Repositories.Interfaces
{
    public interface IPlayerRepository
    {
        public Task<Player> Create(Player player, int teamId);
        public Task<Player?> GetById(int id);
        public Task<List<Player>> GetAllByTeam(int teamId);
    }
}
