﻿using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.DAL.Repositories.Interfaces
{
    public interface ITeamRepository
    {
        public Task<Team?> GetById(int id);
        public Task<List<Team>> GetAllByUser(int userId);
        public Task<List<Team>?> GetAllByLeague(int leagueId);
        public Task<Team> Create(Team team);
    }
}
