using DiamondLegends.API.DTO;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.API.Mappers
{
    public static class TeamMappers
    {
        public static Team ToTeam(this TeamCreationForm teamForm)
        {
            return new Team()
            {
                Name = teamForm.Name,
                Abbreviation = teamForm.Abbreviation,
                City = teamForm.City,
                Logo = teamForm.Logo,
                Color_1 = teamForm.Color_1,
                Color_2 = teamForm.Color_2,
                Color_3 = teamForm.Color_3
            };
        }
        public static TeamView ToView(this Team team)
        {
            return new TeamView()
            {
                Id = team.Id,
                Name = team.Name,
                Abbreviation = team.Abbreviation,
                Owner = team.Owner.ToView(),
                City = team.City,
                Country = team.Country.Name,
                League = team.League.ToViewList(),
                Season = team.Season,
                InGameDate = team.InGameDate,
                Budget = team.Budget,
                Logo = team.Logo,
                Color_1 = team.Color_1,
                Color_2 = team.Color_2,
                Color_3 = team.Color_3,
                Players = team.Players?.Select(p => p.ToViewList()).ToList()
            };
        }

        public static TeamViewList ToViewList(this Team team)
        {
            return new TeamViewList()
            {
                Id = team.Id,
                Name = team.Name,
                Abbreviation = team.Abbreviation,   
                Owner = team.Owner.ToView(),
                Country = team.Country.Name,
                Logo = team.Logo,
                season = team.Season
            };
        }

        public static TeamViewPlayer ToViewPlayer(this Team team)
        {
            return new TeamViewPlayer()
            {
                Id = team.Id,
                Name = team.Name,
                Abbreviation = team.Abbreviation
            };
        }

        public static TeamViewCalendar ToViewCalendar(this Team team)
        {
            return new TeamViewCalendar()
            {
                Id = team.Id,
                Name = team.Name,
                Abbreviation = team.Abbreviation,
            };
        }
    }
}
