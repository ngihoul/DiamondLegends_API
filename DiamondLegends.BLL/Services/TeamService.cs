using DiamondLegends.BLL.Generators;
using DiamondLegends.BLL.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;
using System.Transactions;

namespace DiamondLegends.BLL.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILeagueRepository _leagueRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly LeagueNameGenerator _leagueNameGenerator;
        private readonly TeamGenerator _teamGenerator;

        public TeamService(
            ITeamRepository teamRepository,
            IUserRepository userRepository,
            ILeagueRepository leagueRepository,
            ICountryRepository countryRepository,

            LeagueNameGenerator leagueNameGenerator,
            TeamGenerator teamGenerator
            )
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _leagueRepository = leagueRepository;
            _countryRepository = countryRepository;

            _leagueNameGenerator = leagueNameGenerator;
            _teamGenerator = teamGenerator;
        }

        public async Task<Team> Create(Team team, int userId, int countryId)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // Add Owner = Use HttpContext and pass in parameters
                    User? user = await _userRepository.GetById(userId);
                    if (user is null)
                    {
                        throw new ArgumentException("Cet utilisateur n'existe pas");
                    }
                    team.Owner = user;

                    // Fetch country
                    team.Country = await _countryRepository.GetById(countryId);
                    if (team.Country is null)
                    {
                        throw new ArgumentException("Le pays spécifié n'existe pas");
                    }

                    // Create & Link to a League
                    League newLeague = new League() { Name = _leagueNameGenerator.Generate() };
                    League league = await _leagueRepository.Create(newLeague);

                    team.League = league;

                    // Init Season = currentYear
                    int season = DateTime.Now.Year;
                    team.Season = season;

                    // Create Opponents & link them to the League
                    for (int i = 0; i < 5; i++)
                    {
                        Team opponent = await _teamGenerator.Generate(league, season);
                        await _teamRepository.Create(opponent);
                    }

                    // Create the team and link to league
                    Team createdTeam = await _teamRepository.Create(team);

                    transactionScope.Complete();

                    return createdTeam;
                }
                catch (Exception e)
                {
                    throw new Exception("Une erreur est survenue lors de la création de l'équipe", e);
                }
            }
        }

        public async Task<Team> Get(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException("Cette équipe n'existe pas");
            }

            Team? team = await _teamRepository.GetById(id);

            if (team is null)
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
