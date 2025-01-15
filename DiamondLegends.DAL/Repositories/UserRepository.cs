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

            User newUser = _connection.QuerySingle<User>(
                "INSERT INTO Users(Username, Email, Password, Salt, Nationality) " +
                "OUTPUT INSERTED.* " +
                "VALUES (@Username, @Email, @Password, @Salt, @Nationality)", 
                user
            );

            await _connection.CloseAsync();

            return newUser;
        }
    }
}
