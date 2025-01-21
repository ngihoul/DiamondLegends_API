using DiamondLegends.Domain.Models;

namespace DiamondLegends.API.DTO
{
    public class TeamView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserView Owner { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string League { get; set; }
        public int Season { get; set; }
        public int CurrentDay { get; set; }
        public Int64 Budget { get; set; }
        public string? Logo { get; set; }
        public string Color_1 { get; set; }
        public string Color_2 { get; set; }
        public string Color_3 { get; set; }
        // public IEnumerable<Player> Players { get; set; }
    }

    public class TeamViewList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserView Owner { get; set; }
        public string Country { get; set; }
        public string? Logo { get; set; }
        public int season { get; set; }
    }

    public class TeamCreationForm
    {
        public string Name { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }
        public string? Logo { get; set; }
        public string Color_1 { get; set; }
        public string Color_2 { get; set; }
        public string Color_3 { get; set; }
    }
}
