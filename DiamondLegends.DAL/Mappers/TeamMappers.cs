using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;
using System.Numerics;

namespace DiamondLegends.DAL.Mappers
{
    public static class TeamMappers
    {
        public static Team FullTeam(SqlDataReader reader)
        {
            return new Team()
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                Owner = new User()
                {
                    Id = (int)reader["OwnerId"],
                    Username = (string)reader["Username"],
                    Email = (string)reader["Email"],
                    Nationality = new Country()
                    {
                        Id = (int)reader["OwnerCountryId"],
                        Name = (string)reader["OwnerCountryName"],
                        Alpha2 = (string)reader["OwnerCountryAlpha2"]
                    }
                },
                City = (string)reader["City"],
                Country = new Country()
                {
                    Id = (int)reader["CountryId"],
                    Name = (string)reader["CountryName"],
                    Alpha2 = (string)reader["CountryAlpha2"]
                },
                League = new League()
                {
                    Id = (int)reader["LeagueId"],
                    Name = (string)reader["LeagueName"],
                },
                Season = (int)reader["Season"],
                CurrentDay = (int)reader["CurrentDay"],
                Budget = Convert.ToInt64(reader["Budget"]),
                Logo = reader["Logo"] is DBNull ? null : (string)reader["Logo"],
                Color_1 = (string)reader["Color_1"],
                Color_2 = (string)reader["Color_2"],
                Color_3 = (string)reader["Color_3"]
            };
        }
    }
}
