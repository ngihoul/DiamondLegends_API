using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;
using System.Text;

namespace DiamondLegends.BLL.Generators
{
    public class SeasonGenerator(IGameRepository _gameRepository)
    {
        public const int NB_DAYS_IN_SEASON = 185;

        public async Task<List<Game>> Generate(List<Team> teams)
        {
            int currentSeasonYear = DateTime.Now.Year;
            DateTime startSeasonDate = new DateTime(currentSeasonYear, 03, 27);
            DateTime dayDate = startSeasonDate;

            int NB_TEAMS = teams.Count;
            int NB_TOTAL_DAYS = 7 * (NB_TEAMS - 1);
            int NB_GAMES_PER_DAY = NB_TEAMS / 2;
            int NB_GAMES_PER_SEASON = NB_GAMES_PER_DAY * NB_TOTAL_DAYS;
            double DAYS_BETWEEN_GAMES = Math.Ceiling((double)NB_DAYS_IN_SEASON / NB_TOTAL_DAYS);

            // Using this switch to vary between 3 and 4 days between games (185 / 49 = 3.75)
            // Used to finish the season in September
            bool switchDaysBetween = true;

            List<Game> schedule = new List<Game>();

            for (int day = 0; day < NB_TOTAL_DAYS; day++)
            {
                switchDaysBetween = !switchDaysBetween;

                for (int i = 0; i < NB_TEAMS / 2; i++)
                {
                    Team team1 = teams[i];
                    Team team2 = teams[NB_TEAMS - 1 - i];

                    Game newGame = new Game()
                    {
                        Date = dayDate,
                        Season = currentSeasonYear,
                        Away = day % 2 == 0 ? team1 : team2,
                        Home = day % 2 == 0 ? team2 : team1,
                    };

                    newGame = await _gameRepository.Create(newGame);
                    schedule.Add(newGame);
                }

                Team lastTeam = teams[NB_TEAMS - 1];
                teams.RemoveAt(NB_TEAMS - 1);
                teams.Insert(1, lastTeam);

                dayDate = dayDate.AddDays(switchDaysBetween ? DAYS_BETWEEN_GAMES - 1 : DAYS_BETWEEN_GAMES);
            }

            return schedule;
        }
    }
}


/*EVEN # of teams

When we have an even number of teams, you do the same rotation, except you hold team #1 in fixed position and rotate all the other teams around #1 in a clockwise fashion. So, if we had 4 teams..

1 2 --> 1 3 --> 1 4 
3 4     4 2     2 3 

This would be one complete round robin... the next match up would be..

1 2 
3 4 
*/

/*
8 TEAMS

28 matchs / tour
* 2 tours
= 56 matchs
7 journées / tour
* 4 matchs / journée
SOIT 14 journées

 */

