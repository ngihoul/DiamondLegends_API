using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.DAL.Interfaces
{
    public interface ILeagueRepository
    {
        public Task<League> Create(League league);
    }
}
