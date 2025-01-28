using DiamondLegends.Domain.Enums;

namespace DiamondLegends.Domain.Models
{
    public class GameOffensiveStats
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        public Player Player { get; set; }
        public int Order { get; set; }
        public Position Position { get; set; }
        public double? AVG { get; set; } = 0.000;
        public double? OBP { get; set; } = 0.000;
        public double? SLG { get; set; } = 0.000;
        public double? OPS { get; set; } = 0.000;
        public int AB { get; set; } = 0;
        public int R { get; set; } = 0;
        public int H { get; set; } = 0;
        public int Double { get; set; } = 0;
        public int Triple { get; set; } = 0;
        public int HR { get; set; } = 0;
        public int RBI { get; set; } = 0;
        public int BB { get; set; } = 0;
        public int IBB { get; set; } = 0;
        public int SO { get; set; } = 0;
        public int SB { get; set; } = 0;
        public int CS { get; set; } = 0;
    }
}
