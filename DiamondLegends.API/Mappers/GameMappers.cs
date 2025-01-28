using DiamondLegends.API.DTO;
using DiamondLegends.Domain.Enums;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.API.Mappers
{
    public static class GameMappers
    {
        public static GameView ToView(this Game game)
        {
            return new GameView()
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
                OffensiveStats = game.OffensiveStats,
                PitchingStats = game.PitchingStats
            };
        }

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

        public static List<GameOffensiveStats> ToOffensiveStatList(this GameLineUp lineUp)
        {
            List<GameOffensiveStats> list = new List<GameOffensiveStats>();

            foreach (GameLineUpDetails details in lineUp.LineUpDetails)
            {
                if(details.Position != Position.StartingPitcher)
                {
                    list.Add(new GameOffensiveStats()
                    {
                        Game = lineUp.Game,
                        Player = details.Player,
                        Order = details.Order,
                        Position = details.Position,
                    });
                }
            }

            return list;
        }

        public static GamePitchingStats ToPitchingStat(this GameLineUp lineUp)
        {
            GamePitchingStats startingPitcher = new GamePitchingStats();

            foreach (GameLineUpDetails details in lineUp.LineUpDetails)
            {
                if (details.Position == Position.StartingPitcher)
                {
                    startingPitcher.Game = lineUp.Game;
                    startingPitcher.Player = details.Player;
                }
            }

            return startingPitcher;
        }
    }
}
