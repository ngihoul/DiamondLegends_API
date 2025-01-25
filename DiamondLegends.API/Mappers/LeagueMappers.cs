using DiamondLegends.API.DTO;
using DiamondLegends.Domain.Models;
using System.Runtime.CompilerServices;

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
                InGameDate = league.InGameDate,
                Teams = league.Teams.Select(t => t.ToViewList()).ToList(),
            };
        }

        public static LeagueViewList ToViewList(this League league) {
            return new LeagueViewList()
            {
                Id = league.Id,
                Name = league.Name,
            };
        }
    }
}
