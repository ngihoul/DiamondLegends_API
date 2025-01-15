using DiamondLegends.BLL.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.BLL.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<IEnumerable<Country>> GetAll()
        {
            IEnumerable<Country> countries = await _countryRepository.GetAll();
            
            return countries;
        }
    }
}
