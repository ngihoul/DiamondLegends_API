﻿using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<User> Create(User user);
    }
}
