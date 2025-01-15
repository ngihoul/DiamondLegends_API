using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.BLL.Interfaces
{
    public interface ICountryService
    {
        public Task<IEnumerable<Country>> GetAll();
    }
}
