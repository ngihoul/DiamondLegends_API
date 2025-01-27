using DiamondLegends.API.DTO;
using DiamondLegends.API.Mappers;
using DiamondLegends.BLL.Interfaces;
using DiamondLegends.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLegends.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController(ILeagueService _leagueService) : ControllerBase
    {
        [HttpGet("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LeagueView>> Get([FromRoute] int id)
        {
            League league = await _leagueService.GetById(id);

            return Ok(league.ToView());
        }
    }
}
