﻿using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.DAL.Repositories.Interfaces
{
    public interface ILeagueRepository
    {
        public Task<League> Create(League league);
        public Task<League?> GetById(int id);
        public Task<League?> UpdateInGameDate(League league);
    }
}
