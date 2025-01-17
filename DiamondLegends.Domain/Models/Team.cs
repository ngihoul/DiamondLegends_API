namespace DiamondLegends.Domain.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public Country Country { get; set; }
        public string? Logo { get; set; }
        public string Color_1 { get; set; }
        public string Color_2 { get; set; }
        public string Color_3 { get; set; }
        // public IEnumerable<Player> Players { get; set; }
    }
}
