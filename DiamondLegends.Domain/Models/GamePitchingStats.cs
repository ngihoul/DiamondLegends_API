using System;

namespace DiamondLegends.Domain.Models
{
    public class GamePitchingStats
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        public Player Player { get; set; }
        public decimal? ERA { get; set; }
        public decimal? WHIP { get; set; }
        public decimal? AVG { get; set; }
        public int W { get; set; }
        public int L { get; set; }
        public int G { get; set; }
        public int GS { get; set; }
        public int CG { get; set; }
        public int SHO { get; set; }
        public int HLD { get; set; }
        public int SV { get; set; }
        public int SVO { get; set; }
        public decimal IP { get; set; }
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
