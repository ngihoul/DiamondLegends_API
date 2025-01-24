using DiamondLegends.API.DTO;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.API.Mappers
{
    public static class GameMappers
    {
        public static GameViewList ToViewList(this Game game)
        {
            return new GameViewList()
            {
                Id = game.Id,
                Date = game.Date,
                Season = game.Season,
                Away = game.Away.ToViewCalendar(),
                Home = game.Home.ToViewCalendar(),
                AwayRuns = game.AwayRuns,
                HomeRuns = game.HomeRuns,
                AwayHits = game.AwayHits,
                HomeHits = game.HomeHits,
                AwayErrors = game.AwayErrors,
                HomeErrors = game.HomeErrors,
            };
        }
    }
}
