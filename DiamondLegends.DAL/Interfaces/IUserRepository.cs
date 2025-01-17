using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.DAL.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> Create(User user);
        public Task<User?> GetByUsername(string username);
        public Task<User?> GetByEmail(string email);
    }
}
