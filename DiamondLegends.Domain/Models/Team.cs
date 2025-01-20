using System.Numerics;

namespace DiamondLegends.Domain.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User Owner { get; set; }
        public string City { get; set; }
        public Country Country { get; set; }
        public League League { get; set; }
        public int Season { get; set; }
        public int CurrentDay { get; set; } = 0;
        public Int64 Budget { get; set; } = 1000000;
        public string? Logo { get; set; } = null;
        public string Color_1 { get; set; }
        public string Color_2 { get; set; }
        public string Color_3 { get; set; } = "FFFFFF";
        public List<Player> Players { get; set; } = new List<Player>();
    }
}
