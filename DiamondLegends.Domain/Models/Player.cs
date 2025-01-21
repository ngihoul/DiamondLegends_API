using DiamondLegends.Domain.Enums;

namespace DiamondLegends.Domain.Models
{
    public class Player
    {
        public const int BATTING_RIGHT = 0;
        public const int BATTING_LEFT = 1;

        public const int THROWING_RIGHT = 0;
        public const int THROWING_LEFT = 1;

        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Country Nationality { get; set; }
        public int Throw { get; set; }
        public int Bat { get; set; }
        public decimal Salary { get; set; }
        public int Energy { get; set; } = 100;
        public int Contact { get; set; }
        public int ContactPotential { get; set; }
        public int Power { get; set; }
        public int PowerPotential { get; set; }
        public int Running { get; set; }
        public int RunningPotential { get; set; }
        public int Defense { get; set; }
        public int DefensePotential { get; set; }
        public int Mental { get; set; }
        public int MentalPotential { get; set; }
        public int Stamina { get; set; }
        public int StaminaPotential { get; set; }
        public int Control { get; set; }
        public int ControlPotential { get; set; }
        public int Velocity { get; set; }
        public int VelocityPotential { get; set; }
        public int Movement { get; set; }
        public int MovementPotential { get; set; }
        public IEnumerable<Position> Positions { get; set; }
        public Team Team { get; set; } 
    }
}
