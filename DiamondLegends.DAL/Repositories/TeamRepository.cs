using Dapper;
using DiamondLegends.DAL.Factories.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.DAL.Mappers;
using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;

namespace DiamondLegends.DAL.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly IDbConnectionFactory _connection;

        public TeamRepository(IDbConnectionFactory connection)
        {
            _connection = connection;
        }

        public async Task<Team> Create(Team team)
        {
            using(var connection = _connection.Create()) { 
                await connection.OpenAsync();

                team.Id = await connection.QuerySingleAsync<int>(
                                "INSERT INTO Teams(Name, Owner, City, Country, League, Season, CurrentDay, Budget, Logo, Color_1, Color_2, Color_3) " +
                                "OUTPUT INSERTED.Id " +
                                "VALUES (@Name, @Owner, @City, @Country, @League, @Season, @CurrentDay, @Budget, @Logo, @Color_1, @Color_2, @Color_3)",
                                new { Name = team.Name, Owner = team.Owner.Id, City = team.City, Country = team.Country.Id, League = team.League.Id, Season = team.Season, CurrentDay = team.CurrentDay, Budget = team.Budget, Logo = team.Logo, Color_1 = team.Color_1, Color_2 = team.Color_2, Color_3 = team.Color_3 }
                            );

                return team;
            }
        }

        public async Task<List<Team>?> GetAllByUser(int userId)
        {
            using (var connection = _connection.Create())
            {
                SqlCommand command = connection.CreateCommand();

                List<Team> teams = new List<Team>();

                command.CommandText = "SELECT " +
                    "T.Id, T.Name, T.City, T.Logo, T.Color_1, T.Color_2, T.Color_3, T.Season, T.CurrentDay, T.Budget, " +
                    "U.Id AS OwnerId, U.Username, U.Email, " +
                    "CO.Id AS OwnerCountryId, CO.Name AS OwnerCountryName, CO.Alpha2 AS OwnerCountryAlpha2, " +
                    "L.Id AS LeagueId, L.Name AS LeagueName, " +
                    "C.Id AS CountryId, C.Name AS CountryName, C.Alpha2 AS CountryAlpha2 " +
                    "FROM Teams AS T " +
                    "JOIN Users AS U ON T.Owner = U.Id " +
                    "JOIN Countries AS CO ON U.Nationality = CO.Id " +
                    "JOIN Countries AS C ON T.Country = C.Id " +
                    "JOIN Leagues AS L ON T.League = L.Id " +
                    "WHERE U.Id = @Id";

                command.Parameters.AddWithValue("@Id", userId);

                await connection.OpenAsync();

                using SqlDataReader reader = await command.ExecuteReaderAsync();

                while(await reader.ReadAsync())
                {
                    teams.Add(
                        TeamMappers.FullTeam(reader)
                    );
                }

                return teams;
            }
        }

        public async Task<Team?> GetById(int id)
        {
            using (var connection = _connection.Create()) {
                SqlCommand command = connection.CreateCommand();

                Team? team = null;

                command.CommandText = "SELECT " +
                    "T.Id, T.Name, T.City, T.Logo, T.Color_1, T.Color_2, T.Color_3, T.Season, T.CurrentDay, T.Budget, " +
                    "U.Id AS OwnerId, U.Username, U.Email, " +
                    "CO.Id AS OwnerCountryId, CO.Name AS OwnerCountryName, CO.Alpha2 AS OwnerCountryAlpha2, " +
                    "L.Id AS LeagueId, L.Name AS LeagueName, " +
                    "C.Id AS CountryId, C.Name AS CountryName, C.Alpha2 AS CountryAlpha2 " +
                    "FROM Teams AS T " +
                    "JOIN Users AS U ON T.Owner = U.Id " +
                    "JOIN Countries AS CO ON U.Nationality = CO.Id " +
                    "JOIN Countries AS C ON T.Country = C.Id " +
                    "JOIN Leagues AS L ON T.League = L.Id " +
                    "WHERE T.Id = @Id";

                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();

                using SqlDataReader reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    team = TeamMappers.FullTeam(reader);
                }

                return team;
            }
        }
    }
}
