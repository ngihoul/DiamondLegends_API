using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.DAL.Mappers
{
    public static class LeagueMappers
    {
        public static League FullLeague(SqlDataReader reader, List<Team> teams)
        {
            return new League()
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                Teams = teams
            };
        }
    }
}
