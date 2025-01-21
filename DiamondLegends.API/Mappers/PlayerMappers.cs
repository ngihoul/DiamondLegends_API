using DiamondLegends.API.DTO;
using DiamondLegends.Domain.Enums;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.API.Mappers
{
    public static class PlayerMappers
    {
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
                Positions = player.Positions,
            };
        }
    }
}
