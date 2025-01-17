using System.Numerics;

namespace DiamondLegends.Domain.Models
{
    public class Franchise
    {
        public string Id { get; set; }
        public User Owner { get; set; }
        public Team Team { get; set; }
        public League League { get; set; }
        public int Season { get; set; }
        public string CurrentDay { get; set; }
        public BigInteger Budget { get; set; }
    }
}
