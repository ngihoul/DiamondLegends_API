using DiamondLegends.BLL.Interfaces;
using DiamondLegends.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DiamondLegends.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Country>>> GetAll()
        {
            IEnumerable<Country> countries = await _countryService.GetAll();

            return Ok(countries);
        }
    }
}
