using DiamondLegends.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace DiamondLegends.Domain.Models
{
    public class Game
    {
        public static int TO_BE_PLAYED = 0;
        public static int PLAYED = 1;

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Season { get; set; }
        public int Status { get; set; } = 0;
        public Team Away { get; set; }
        public Team Home { get; set; }
        public int HalfInnings { get; set; } = 0;
        public int AwayRuns { get; set; } = 0;
        public int HomeRuns { get; set; } = 0;
        public int AwayHits { get; set; } = 0;
        public int HomeHits { get; set; } = 0;
        public int AwayErrors { get; set; } = 0;
        public int HomeErrors { get; set; } = 0;
        public List<GameOffensiveStats>? OffensiveStats { get; set; }
        public List<GamePitchingStats>? PitchingStats { get; set; }
    }

    public class GameQuery
    {
        public int? Season { get; set; } = null;
        public int? LeagueId { get; set; } = null;
        public int? TeamId { get; set; } = null;
        public int? Month { get; set; } = null;
        public int? Day { get; set; } = null;
    }

    public class GameLineUp
    {
        public List<GameLineUpDetails> LineUpDetails { get; set; }
    }

    public class GameLineUpDetails
    {
        public int PlayerId { get; set; }
        public int Order { get; set; }
        public Position Position { get; set; }
    }

    public class GameEvent
    {
        
        public string Message { get; set; } 
        public int Outs { get; set; } 
        public int Strikes { get; set; } 
        public int Balls { get; set; } 
        public int HalfInnings { get; set; } 
        public int RunsAway { get; set; } 
        public int RunsHome { get; set; }    }
}
