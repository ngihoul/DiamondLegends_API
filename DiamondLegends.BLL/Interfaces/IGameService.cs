using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.BLL.Interfaces
{
    public interface IGameService
    {
        public Task<List<Game>> GetAll(GameQuery? query = null);
    }
}
