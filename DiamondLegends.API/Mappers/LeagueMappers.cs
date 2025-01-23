using DiamondLegends.API.DTO;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.API.Mappers
{
    public static class LeagueMappers
    {
        public static LeagueView ToView(this League league)
        {
            return new LeagueView()
            {
                Id = league.Id,
                Name = league.Name,
                Teams = league.Teams.Select(t => t.ToViewList()).ToList(),
            };
        }
    }
}
