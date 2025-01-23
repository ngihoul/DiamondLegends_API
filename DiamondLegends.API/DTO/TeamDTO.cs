using DiamondLegends.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace DiamondLegends.API.DTO
{
    public class TeamView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public UserView Owner { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public LeagueViewList League { get; set; }
        public int Season { get; set; }
        public int CurrentDay { get; set; }
        public Int64 Budget { get; set; }
        public string? Logo { get; set; }
        public string Color_1 { get; set; }
        public string Color_2 { get; set; }
        public string Color_3 { get; set; }
        public List<PlayerViewList>? Players { get; set; }
    }

    public class TeamViewList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public UserView Owner { get; set; }
        public string Country { get; set; }
        public string? Logo { get; set; }
        public int season { get; set; }
    }

    public class TeamCreationForm
    {
        // TODO : ajouter annotations
        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(5)]
        public string Abbreviation { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public int CountryId { get; set; }
        public string? Logo { get; set; }

        [Required]
        public string Color_1 { get; set; }

        [Required]
        public string Color_2 { get; set; }
        public string Color_3 { get; set; }
    }

    public class TeamViewPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }
}
