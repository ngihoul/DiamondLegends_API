namespace DiamondLegends.Domain.Models
{
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime InGameDate { get; set; }
        public List<Team>? Teams { get; set; } = new List<Team>();
        public List<Game> Games { get; set; } = new List<Game>();
    }
}
