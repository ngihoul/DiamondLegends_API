using Dapper;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;

namespace DiamondLegends.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection _connection;

        public UserRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<User> Create(User user)
        {
            await _connection.OpenAsync();

            user.Id = _connection.QuerySingle<int>(
                "INSERT INTO Users(Username, Email, Password, Salt, Nationality) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@Username, @Email, @Password, @Salt, @Nationality)", 
                new { user.Username, user.Email, user.Password, user.Salt, Nationality = user.Nationality.Id }
            );

            await _connection.CloseAsync();

            return user;
        }
    }
}
