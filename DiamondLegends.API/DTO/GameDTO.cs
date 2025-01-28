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
        public List<GameOffensiveStats>? OffensiveStats { get; set; }
        public List<GamePitchingStats>? PitchingStats { get; set; }
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

    public class GameLineUp
    {
        [Required]
        public Game Game { get; set; }

        [Required]
        public List<GameLineUpDetails> LineUpDetails { get; set; }
    }

    public class GameLineUpDetails
    {
        [Required]
        public Player Player { get; set; }
        [Required]
        [Range(1, 9)]
        public int Order { get; set; }


        [Required]
        [Range(1, 9)]
        public Position Position { get; set; }
    }
}
