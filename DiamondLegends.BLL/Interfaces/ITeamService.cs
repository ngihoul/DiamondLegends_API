using DiamondLegends.Domain.Models;

namespace DiamondLegends.BLL.Interfaces
{
    public interface ITeamService
    {
        public Task<Team> Get(int id);
        public Task<List<Team>?> GetAllByUser(int userId);
        public Task<Team> Create(Team team, int userId, int countryId);
    }
}
