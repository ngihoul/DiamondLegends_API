using Dapper;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;

namespace DiamondLegends.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection _connection;
        private readonly ICountryRepository _countryRepository;

        public UserRepository(SqlConnection connection, ICountryRepository countryRepository)
        {
            _connection = connection;
            _countryRepository = countryRepository;
        }

        public async Task<User> Create(User user)
        {
            await _connection.OpenAsync();

            user.Id = await _connection.QuerySingleAsync<int>(
                "INSERT INTO Users(Username, Email, Password, Salt, Nationality) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@Username, @Email, @Password, @Salt, @Nationality)", 
                new { user.Username, user.Email, user.Password, user.Salt, Nationality = user.Nationality.Id }
            );

            await _connection.CloseAsync();

            return user;
        }

        public async Task<User?> GetByUsername(string username)
        {
            await _connection.OpenAsync();

            var results = await _connection.QueryAsync<User?, Country?, User?>(
                "SELECT * FROM Users WHERE Username = @Username",
                (user, country) =>
                {
                    user.Nationality = country;
                    return user;
                },
                new { Username = username },
                splitOn: "Nationality"
            );

            User? user = results.FirstOrDefault();

            await _connection.CloseAsync();

            return user;
        }

        public async Task<User?> GetByEmail(string email)
        {
            await _connection.OpenAsync();

            var results = await _connection.QueryAsync<User?, Country?, User?>(
                "SELECT * FROM Users WHERE Email = @Email",
                (user, country) =>
                {
                    user.Nationality = country;
                    return user;
                },
                new { Email = email },
                splitOn: "Nationality"
            );

            User? user = results.FirstOrDefault();

            await _connection.CloseAsync();

            return user;
        }
    }
}
