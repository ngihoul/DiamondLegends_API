using DiamondLegends.API.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DiamondLegends.API.Mappers;
using DiamondLegends.Domain.Models;
using DiamondLegends.BLL.Interfaces;
using DiamondLegends.API.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DiamondLegends.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TeamView>> Get([FromRoute] int id)
        {
            Team team = await _teamService.Get(id);

            return Ok(team.ToView());
        }

        [HttpGet("user")]
        public async Task<ActionResult<List<TeamViewList>?>> GetAllByUser()
        {
            int userId = User.GetUserId();

            // TODO : Utiliser isConnected ?
            if(userId <= 0)
            {
                throw new ArgumentException("L'utilisateur n'est pas connecté");
            }

            List<Team>? teams = await _teamService.GetAllByUser(userId);

            return Ok(teams?.Select(t => t.ToViewList()));
        }

        [HttpPost]
        public async Task<ActionResult<TeamView>> Create([FromBody] TeamCreationForm teamForm)
        { 
            if(teamForm is null || !ModelState.IsValid)
            {
                throw new ArgumentNullException("L'équipe n'est pas valide'");
            }

            int userId = User.GetUserId();

            // TODO : Utiliser isConnected ?
            if (userId <= 0)
            {
                throw new ArgumentException("L'utilisateur n'est pas connecté");
            }

            Team team = await _teamService.Create(teamForm.ToTeam(), userId, teamForm.CountryId);

            return Ok(team.ToView());
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
