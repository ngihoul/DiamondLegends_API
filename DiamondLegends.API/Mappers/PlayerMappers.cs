using DiamondLegends.API.DTO;
using DiamondLegends.Domain.Models;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace DiamondLegends.API.Mappers
{
    public static class PlayerMappers
    {
        public static PlayerView ToView(this Player player)
        {
            return new PlayerView() {
                Id = player.Id,
                Firstname = player.Firstname,
                Lastname = player.Lastname,
                DateOfBirth = player.DateOfBirth,
                Nationality = player.Nationality,
                Throw = player.Throw,
                Bat = player.Bat,
                Salary = player.Salary,
                Energy = player.Energy,
                Contact = player.Contact,
                Power = player.Power,
                Running = player.Running,
                Defense = player.Defense,
                Mental = player.Mental,
                Stamina = player.Stamina,
                Control = player.Control,
                Velocity = player.Velocity,
                Movement = player.Movement,
                Positions = player.Positions,
                Team = player.Team.ToViewPlayer()
            };
        }
        public static PlayerViewList ToViewList(this Player player)
        {
            return new PlayerViewList()
            {
                Id = player.Id,
                Firstname = player.Firstname,
                Lastname = player.Lastname,
                Nationality = player.Nationality,
                Throw = player.Throw,
                Bat = player.Bat,
                Energy = player.Energy,
                Positions = player.Positions
            };
        }

        public static PlayerViewGameRecap ToViewGameRecap(this Player player)
        {
            return new PlayerViewGameRecap()
            {
                Id = player.Id,
                Firstname = player.Firstname,
                Lastname = player.Lastname
            };
        }
    }
}
