﻿using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.BLL.Interfaces
{
    public interface ILeagueService
    {
        public Task<League> GetById(int id);
        public Task<League> NextDay(int leagueId);
        public Task<League> NextGame(int leagueId);
    }
}
