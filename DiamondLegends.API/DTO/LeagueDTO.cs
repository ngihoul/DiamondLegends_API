using DiamondLegends.Domain.Models;

namespace DiamondLegends.API.DTO
{
    public class LeagueView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TeamViewList> Teams { get; set; } = new List<TeamViewList>();
    }

    public class LeagueViewList
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
