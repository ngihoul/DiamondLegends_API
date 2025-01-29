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
                OffensiveStats = game.OffensiveStats.Select(o => o.ToView()).ToList(),
                PitchingStats = game.PitchingStats.Select(p => p.ToView()).ToList()
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

        public static OffensiveStatsView ToView(this GameOffensiveStats offensiveStats)
        {
            return new OffensiveStatsView()
            {
                Id = offensiveStats.Id,
                Player = offensiveStats.Player.ToViewGameRecap(),
                Order = offensiveStats.Order,
                Position = offensiveStats.Position,
                AVG = offensiveStats.AVG,
                OBP = offensiveStats.OBP,
                SLG = offensiveStats.SLG,
                OPS = offensiveStats.OPS,
                PA = offensiveStats.PA,
                AB = offensiveStats.AB,
                R = offensiveStats.R,
                H = offensiveStats.H,
                Double = offensiveStats.Double,
                Triple = offensiveStats.Triple,
                HR = offensiveStats.HR,
                RBI = offensiveStats.RBI,
                BB = offensiveStats.BB,
                IBB = offensiveStats.IBB,
                SO = offensiveStats.SO,
                SB = offensiveStats.SB,
                CS = offensiveStats.CS,
            };
        }

        public static PitchingStatsView ToView(this GamePitchingStats pitchingStats)
        {
            return new PitchingStatsView()
            {
                Id = pitchingStats.Id,
                Player = pitchingStats.Player.ToViewGameRecap(),
                ERA = pitchingStats.ERA,
                WHIP = pitchingStats.WHIP,
                AVG = pitchingStats.AVG,
                W = pitchingStats.W,
                L = pitchingStats.L,
                G = pitchingStats.G,
                GS = pitchingStats.GS,
                CG = pitchingStats.CG,
                SHO = pitchingStats.SHO,
                HLD = pitchingStats.HLD,
                SV = pitchingStats.SV,
                SVO = pitchingStats.SVO,
                IP = pitchingStats.IP,
                H = pitchingStats.H,
                R = pitchingStats.R,
                ER = pitchingStats.ER,
                HR = pitchingStats.HR,
                NP = pitchingStats.NP,
                HB = pitchingStats.HB,
                BB = pitchingStats.BB,
                IBB = pitchingStats.IBB,
                SO = pitchingStats.SO,
            };
        }
    }
}
