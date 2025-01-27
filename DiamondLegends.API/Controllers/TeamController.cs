using DiamondLegends.API.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DiamondLegends.API.Mappers;
using DiamondLegends.Domain.Models;
using DiamondLegends.BLL.Interfaces;
using DiamondLegends.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using static Dapper.SqlMapper;

namespace DiamondLegends.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController(ITeamService _teamService) : ControllerBase
    {
        [HttpGet("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TeamView>> Get([FromRoute] int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException("L'id n'est pas valable.");
            }

            Team team = await _teamService.Get(id);

            return Ok(team.ToView());
        }

        [HttpGet("user")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<TeamViewList>?>> GetAllByUser()
        {
            int userId = User.GetUserId();

            // TODO : Utiliser isConnected ?
            if(userId <= 0)
            {
                throw new UnauthorizedAccessException("L'utilisateur n'est pas connecté.");
            }

            List<Team>? teams = await _teamService.GetAllByUser(userId);

            return Ok(teams?.Select(t => t.ToViewList()));
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TeamView>> Create([FromBody] TeamCreationForm teamForm)
        { 
            if(teamForm is null || !ModelState.IsValid)
            {
                throw new ArgumentNullException("L'équipe n'est pas valide.");
            }

            int userId = User.GetUserId();

            // TODO : Utiliser isConnected ?
            if (userId <= 0)
            {
                throw new UnauthorizedAccessException("L'utilisateur n'est pas connecté.");
            }

            Team team = await _teamService.Create(teamForm.ToTeam(), userId, teamForm.CountryId);

            return CreatedAtAction(nameof(Get), new { id = team.ToView().Id }, team.ToView());
            
        }

        private bool isConnected(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("L'utilisateur n'est pas connecté");
            }

            return true;
        }
    }
}
