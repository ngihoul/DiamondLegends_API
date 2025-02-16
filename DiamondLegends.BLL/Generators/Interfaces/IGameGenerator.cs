﻿using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.BLL.Generators.Interfaces
{
    public interface IGameGenerator
    {
        public Task<Game> Simulate(Game game);
    }
}
