using DiamondLegends.BLL.Interfaces;
using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.BLL.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public Task<Team> Create(Team team)
        {
            throw new NotImplementedException();
        }

        public async Task<Team> Get(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentNullException("Cette équipe n'existe pas");
            }

            Team? team = await _teamRepository.GetById(id);

            if(team is null)
            {
                throw new ArgumentException("Cette équipe n'existe pas");
            }

            return team;
        }

        public Task<IEnumerable<Team>> GetAllByUser(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
