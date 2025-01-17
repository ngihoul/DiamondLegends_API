using DiamondLegends.BLL.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.BLL.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;

        public TeamService(ITeamRepository teamRepository, IUserRepository userRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
        }

        public async Task<Team> Create(Team team, int userId)
        {
            // Add Owner = Use HttpContext and pass in parameters
            User? user = await _userRepository.GetById(userId);

            if(user is null)
            {
                throw new ArgumentException("Cet utilisateur n'existe pas");
            }

            // Create & Link to a League
            // Create Oponnents & link them to the League
            // Init Season = currentYear
            // Init CurrentDay & Budget = automatic ?
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
