using DiamondLegends.BLL.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.BLL.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly IGameRepository _gameRepository;

        public LeagueService(ILeagueRepository leagueRepository, IGameRepository gameRepository)
        {
            _leagueRepository = leagueRepository;
            _gameRepository = gameRepository;
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

        public async Task<League> NextDay(int leagueId)
        {
            return await AdvanceLeague(leagueId, (date, _) => date.AddDays(1));
        }

        public async Task<League> NextGame(int leagueId)
        {
            return await AdvanceLeague(leagueId, (date, nextGameDay) =>
            {
                while (date < nextGameDay)
                {
                    date = date.AddDays(1);
                }
                return date;
            });
        }
        private async Task<League> AdvanceLeague(int leagueId, Func<DateTime, DateTime, DateTime> dateAdvancer)
        {
            League league = await GetById(leagueId);

            if (league is null)
            {
                throw new ArgumentException("La ligue n'existe pas.");
            }

            league.Games = await _gameRepository.GetAll(new GameQuery() { LeagueId = league.Id });

            DateTime nextGameDay = league.Games.Where(g => g.Status == Game.TO_BE_PLAYED).OrderBy(g => g.Date).First().Date;

            if (league.InGameDate >= nextGameDay)
            {
                throw new GameToBePlayedException("Un match doit être joué avant de pouvoir avancer");
            }

            league.InGameDate = dateAdvancer(league.InGameDate, nextGameDay);

            if (await _leagueRepository.UpdateInGameDate(league) is null)
            {
                throw new ArgumentException("Impossible de mettre la ligue à jour.");
            }

            return league;
        }
    }
}
