using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.BLL.Services.Interfaces
{
    public interface ILeagueService
    {
        public Task<League> GetById(int id);
        public Task<League> NextDay(int leagueId, int teamId);
        public Task<League> NextGame(int leagueId, int teamId);
    }
}
