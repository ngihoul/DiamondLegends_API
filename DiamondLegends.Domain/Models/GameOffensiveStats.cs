namespace DiamondLegends.Domain.Models
{
    public class GameOffensiveStats
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        public Player Player { get; set; }
        public int Order { get; set; }
        public int Position { get; set; }
        public decimal? AVG { get; set; }
        public decimal? OBP { get; set; }
        public decimal? SLG { get; set; }
        public decimal? OPS { get; set; }
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
}
