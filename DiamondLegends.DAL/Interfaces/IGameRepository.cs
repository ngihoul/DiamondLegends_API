using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.DAL.Interfaces
{
    public interface IGameRepository
    {
        public Task<Game> Create(Game game);
        public Task<Game?> GetById(int id);
        public Task<List<Game>> GetAll(GameQuery query);
        public Task<Game> Update(Game game);
    }
}
