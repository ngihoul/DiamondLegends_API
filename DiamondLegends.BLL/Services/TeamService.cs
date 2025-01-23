using DiamondLegends.BLL.Generators;
using DiamondLegends.BLL.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;
using System.Transactions;

namespace DiamondLegends.BLL.Services
{
    public class TeamService : ITeamService
    {
        #region Constants
        private const int NB_OPPONENTS = 7;
        #endregion
        
        #region Dependencies
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILeagueRepository _leagueRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly LeagueNameGenerator _leagueNameGenerator;
        private readonly TeamGenerator _teamGenerator;
        private readonly SeasonGenerator _seasonGenerator;
        #endregion

        #region Constructor
        public TeamService(
            ITeamRepository teamRepository,
            IUserRepository userRepository,
            ILeagueRepository leagueRepository,
            ICountryRepository countryRepository,

            LeagueNameGenerator leagueNameGenerator,
            TeamGenerator teamGenerator
,
            SeasonGenerator seasonGenerator)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _leagueRepository = leagueRepository;
            _countryRepository = countryRepository;

            _leagueNameGenerator = leagueNameGenerator;
            _teamGenerator = teamGenerator;
            _seasonGenerator = seasonGenerator;
        }
        #endregion

        #region Methods
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
                    Country? country = await _countryRepository.GetById(countryId);

                    if (country is null) { 
                        throw new ArgumentException("Le pays n'existe pas");
                    }

                    team.Country = country;

                    // Create & Link to a League
                    League newLeague = new League() { Name = _leagueNameGenerator.Generate() };
                    League league = await _leagueRepository.Create(newLeague);

                    team.League = league;

                    // Init Season = currentYear
                    int season = DateTime.Now.Year;
                    team.Season = season;

                    // Create Opponents & link them to the League
                    for (int i = 0; i < NB_OPPONENTS; i++)
                    {
                        Team opponent = await _teamGenerator.Generate(league, season);
                    }

                    // Create the team and link to league
                    Team createdTeam = await _teamRepository.Create(team);

                    // Create Roster
                    team.Players = await _teamGenerator.GenerateRoster(team);

                    // Generate Games for League
                    List<Team>? teamsInLeague = await _teamRepository.GetAllByLeague(league.Id);

                    if(teamsInLeague is null)
                    {
                        throw new Exception("Une erreur est survenue lors de la création de l'équipe");
                    }

                    league.Games = _seasonGenerator.Generate(teamsInLeague);
                    
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

        public async Task<List<Team>?> GetAllByUser(int userId)
        {
            User? user = await _userRepository.GetById(userId);

            if(user is null)
            {
                throw new ArgumentException("Cet utilisateur n'existe pas");
            }

            List<Team>? teams = await _teamRepository.GetAllByUser(userId);

            return teams;
        }
        #endregion
    }
}
