using DiamondLegends.Domain.Enums;
using DiamondLegends.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace DiamondLegends.API.DTO
{
    public class GameView
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Season { get; set; }
        public TeamViewCalendar Away { get; set; }
        public TeamViewCalendar Home { get; set; }
        public int AwayRuns { get; set; } = 0;
        public int HomeRuns { get; set; } = 0;
        public int AwayHits { get; set; } = 0;
        public int HomeHits { get; set; } = 0;
        public int AwayErrors { get; set; } = 0;
        public int HomeErrors { get; set; } = 0;
        public List<OffensiveStatsView>? OffensiveStats { get; set; }
        public List<PitchingStatsView>? PitchingStats { get; set; }
    }

    public class GameViewList
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Season { get; set; }
        public TeamViewCalendar Away { get; set; }
        public TeamViewCalendar Home { get; set; }
        public int AwayRuns { get; set; } = 0;
        public int HomeRuns { get; set; } = 0;
        public int AwayHits { get; set; } = 0;
        public int HomeHits { get; set; } = 0;
        public int AwayErrors { get; set; } = 0;
        public int HomeErrors { get; set; } = 0;
    }

    public class OffensiveStatsView
    {
        public int Id { get; set; }
        public PlayerViewGameRecap Player { get; set; }
        public int Order { get; set; }
        public Position Position { get; set; }
        public double? AVG { get; set; }
        public double? OBP { get; set; }
        public double? SLG { get; set; }
        public double? OPS { get; set; }
        public int AB { get; set; }
        public int R { get; set; }
        public int H { get; set; }
        public int Double { get; set; }
        public int Triple { get; set; }
        public int HR { get; set; }
        public int RBI { get; set; }
        public int BB { get; set; }
        public int IBB { get; set; }
        public int SO { get; set; }
        public int SB { get; set; }
        public int CS { get; set; }
    }

    public class PitchingStatsView
    {
        public int Id { get; set; }
        public PlayerViewGameRecap Player { get; set; }
        public double? ERA { get; set; }
        public double? WHIP { get; set; }
        public double? AVG { get; set; }
        public int W { get; set; }
        public int L { get; set; }
        public int G { get; set; }
        public int GS { get; set; }
        public int CG { get; set; }
        public int SHO { get; set; }
        public int HLD { get; set; }
        public int SV { get; set; }
        public int SVO { get; set; }
        public double IP { get; set; }
        public int H { get; set; }
        public int R { get; set; }
        public int ER { get; set; }
        public int HR { get; set; }
        public int NP { get; set; }
        public int HB { get; set; }
        public int BB { get; set; }
        public int IBB { get; set; }
        public int SO { get; set; }
    }
}
