using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.DAL.Interfaces
{
    public interface IPlayerRepository
    {
        public Task<Player> Create(Player player, int teamId);
        public Task<Player?> GetById(int id);
        public Task<List<Player>> GetAllByTeam(int teamId);
    }
}
