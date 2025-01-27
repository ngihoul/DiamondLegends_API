using DiamondLegends.BLL.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.BLL.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly ILeagueRepository _leagueRepository;

        public LeagueService(ILeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }

        public async Task<League> GetById(int id)
        {
            League? league = await _leagueRepository.GetById(id);

            if (league is null)
            {
                throw new ArgumentException("La ligue n'existe pas.");
            }

            return league;
        }
    }
}
