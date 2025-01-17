using DiamondLegends.DAL.Interfaces;
using DiamondLegends.DAL.Mappers;
using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;

namespace DiamondLegends.DAL.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly SqlConnection _connection;

        public TeamRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public Task<Team> Create(Team team)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Team>> GetAllByUser(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Team> GetById(int id)
        {
            SqlCommand command = _connection.CreateCommand();

            Team? team = null;

            command.CommandText = "SELECT T.Id, T.Name, U.Id AS OwnerId, U.Username, U.Email, CO.Id AS OwnerCountryId, CO.Name AS OwnerCountryName, CO.Alpha2 AS OwnerCountryAlpha2, T.City, T.Logo, T.Color_1, T.Color_2, T.Color_3, L.Id AS LeagueId, L.Name AS LeagueName, T.Season, T.CurrentDay, T.Budget, C.Id AS CountryId, C.Name AS CountryName, C.Alpha2 AS CountryAlpha2 FROM Teams AS T " +
                "JOIN Users AS U ON T.Owner = U.Id " +
                "JOIN Countries AS CO ON U.Nationality = CO.Id " +
                "JOIN Countries AS C ON T.Country = C.Id " +
                "JOIN Leagues AS L ON T.League = L.Id " +
                "WHERE T.Id = @Id";

            command.Parameters.AddWithValue("@Id", id);

            await _connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                team = TeamMappers.TeamWithCountryAndLeague(reader);
            }

            await _connection.CloseAsync();

            return team;
        }
    }
}
