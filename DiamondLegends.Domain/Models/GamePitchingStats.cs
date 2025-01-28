using System;

namespace DiamondLegends.Domain.Models
{
    public class GamePitchingStats
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        public Player Player { get; set; }
        public double? ERA { get; set; } = 0.000;
        public double? WHIP { get; set; } = 0.000;
        public double? AVG { get; set; } = 0.000;
        public int W { get; set; } = 0;
        public int L { get; set; } = 0;
        public int G { get; set; } = 0;
        public int GS { get; set; } = 0;
        public int CG { get; set; } = 0;
        public int SHO { get; set; } = 0;
        public int HLD { get; set; } = 0;
        public int SV { get; set; } = 0;
        public int SVO { get; set; } = 0;
        public double IP { get; set; } = 0.000;
        public int H { get; set; } = 0;
        public int R { get; set; } = 0;
        public int ER { get; set; } = 0;
        public int HR { get; set; } = 0;
        public int NP { get; set; } = 0;
        public int HB { get; set; } = 0;
        public int BB { get; set; } = 0;
        public int IBB { get; set; } = 0;
        public int SO { get; set; } = 0;
    }
}
