﻿using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.BLL.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User> Register(User user, int countryId);
        public Task<string> Login(string emailOrUsername, string password);

        public Task<User> GetById(int id);
    }
}
