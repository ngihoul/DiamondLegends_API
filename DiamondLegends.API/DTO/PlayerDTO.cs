using DiamondLegends.Domain.Enums;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.API.DTO
{
    public class PlayerViewList
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Country Nationality { get; set; }
        public int Throw { get; set; }
        public int Bat { get; set; }
        public int Energy { get; set; } = 100;
        public IEnumerable<Position> Positions { get; set; }
    }
}
