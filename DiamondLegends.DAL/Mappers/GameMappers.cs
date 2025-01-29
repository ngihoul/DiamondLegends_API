using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.DAL.Mappers
{
    public static class GameMappers
    {
        public static Game FullGame(SqlDataReader reader)
        {
            return new Game()
            {
                Id = (int)reader["Id"],
                Date = (DateTime)reader["Date"],
                Season = (int)reader["Season"],
                Home = new Team()
                {
                    Id = (int)reader["HomeId"],
                    Name = (string)reader["HomeName"],
                    Abbreviation = (string)reader["HomeAbbreviation"],
                },
                Away = new Team()
                {
                    Id = (int)reader["AwayId"],
                    Name = (string)reader["AwayName"],
                    Abbreviation = (string)reader["AwayAbbreviation"],
                },
                HomeRuns = (int)reader["Home_runs"],
                AwayRuns = (int)reader["Away_runs"],
                HomeHits = (int)reader["Home_hits"],
                AwayHits = (int)reader["Away_hits"],
                HomeErrors = (int)reader["Home_errors"],
                AwayErrors = (int)reader["Away_errors"]
            };
        }
    }
}
