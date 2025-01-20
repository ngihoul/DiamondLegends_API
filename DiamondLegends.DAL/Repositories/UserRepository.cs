using Dapper;
using DiamondLegends.DAL.Factories.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;

namespace DiamondLegends.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connection;

        public UserRepository(IDbConnectionFactory connection)
        {
            _connection = connection;
        }

        public async Task<User> Create(User user)
        {
            using(var connection = _connection.Create())
            {
                await connection.OpenAsync();

                user.Id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO Users(Username, Email, Password, Salt, Nationality) " +
                    "OUTPUT INSERTED.Id " +
                    "VALUES (@Username, @Email, @Password, @Salt, @Nationality)",
                    new { user.Username, user.Email, user.Password, user.Salt, Nationality = user.Nationality.Id }
                );

                return user;
            }
        }

        // TODO : tester si null est renvoyé ?!
        public async Task<User?> GetByUsername(string username)
        {
            return await GetBy("username", username);
        }

        // TODO : tester si null est renvoyé ?!
        public async Task<User?> GetByEmail(string email)
        {
            return await GetBy("email", email);
        }

        // TODO : tester si null est renvoyé ?!
        public async Task<User?> GetById(int id)
        {
            return await GetBy("id", id.ToString());
        }

        private async Task<User?> GetBy(string column, string value)
        {
            using(var connection = _connection.Create())
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT " +
                        "U.Id, U.Username, U.Email, U.Password, U.Salt, " +
                        "C.Id AS CountryId, C.Name AS CountryName, C.Alpha2 AS CountryAlpha2 " +
                    "FROM Users AS U " +
                    "INNER JOIN Countries AS C ON U.Nationality = C.Id " +
                    $"WHERE U.{column} = @{column}";

                int valueInt = 0;

                if (column == "id")
                {
                    valueInt = Convert.ToInt32(value);
                }

                command.Parameters.AddWithValue($"@{column}", (column == "id") ? valueInt : value);

                await connection.OpenAsync();

                using SqlDataReader reader = await command.ExecuteReaderAsync();

                User? user = null;

                if (await reader.ReadAsync())
                {
                    user = new User
                    {
                        Id = (int)reader["Id"],
                        Username = (string)reader["Username"],
                        Email = reader["Email"] == DBNull.Value ? null : (string)reader["Email"],
                        Password = reader["Password"] == DBNull.Value ? null : (string)reader["Password"],
                        Salt = reader["Salt"] == DBNull.Value ? null : (string)reader["Salt"],
                        Nationality = new Country
                        {
                            Id = reader["CountryId"] == DBNull.Value ? 0 : (int)reader["CountryId"],
                            Name = reader["CountryName"] == DBNull.Value ? null : (string)reader["CountryName"],
                            Alpha2 = reader["CountryAlpha2"] == DBNull.Value ? null : (string)reader["CountryAlpha2"]
                        }
                    };
                }

                return user;
            }
        }
    }
}
