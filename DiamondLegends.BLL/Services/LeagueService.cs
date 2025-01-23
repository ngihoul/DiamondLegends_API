using DiamondLegends.BLL.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (id <= 0)
            {
                // TODO : harmoniser message d'erreur - CustomException ?!
                throw new ArgumentNullException("L'id ne peut pas être null");
            }

            League? league = await _leagueRepository.GetById(id);

            if (league == null)
            {
                throw new ArgumentNullException("La ligue n'existe pas");
            }

            return league;
        }
    }
}
