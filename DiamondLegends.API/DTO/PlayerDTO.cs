using DiamondLegends.Domain.Enums;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.API.DTO
{
    public class PlayerView
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Country Nationality { get; set; }
        public int Throw { get; set; }
        public int Bat { get; set; }
        public decimal Salary { get; set; }
        public int Energy { get; set; }
        public int Contact { get; set; }
        public int Power { get; set; }
        public int Running { get; set; }
        public int Defense { get; set; }
        public int Mental { get; set; }
        public int Stamina { get; set; }
        public int Control { get; set; }
        public int Velocity { get; set; }
        public int Movement { get; set; }
        public IEnumerable<Position> Positions { get; set; }
        public TeamViewPlayer? Team { get; set; }
    }
    
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
        public decimal? AVG { get; set; }
    }

    public class PlayerViewGameRecap
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
