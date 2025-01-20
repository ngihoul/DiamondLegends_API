using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.DAL.Interfaces
{
    public interface ICountryRepository
    {
        public Task<IEnumerable<Country>> GetAll();
        public Task<Country?> GetById(int id);
    }
}
