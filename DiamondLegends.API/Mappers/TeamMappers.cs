using DiamondLegends.API.DTO;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.API.Mappers
{
    public static class TeamMappers
    {
        public static TeamView ToView(this Team team)
        {
            return new TeamView()
            {
                Id = team.Id,
                Name = team.Name,
                City = team.City,
                Country = team.Country.Name,
                Logo = team.Logo,
                Color_1 = team.Color_1,
                Color_2 = team.Color_2,
                Color_3 = team.Color_3
                // public IEnumerable<Player> Players { get; set; }
            };
        }

        public static TeamViewList ToViewList(this Team team)
        {
            return new TeamViewList()
            {
                Id = team.Id,
                Name = team.Name,
                Country = team.Country.Name,
                Logo = team.Logo,
            };
        }
    }
}
