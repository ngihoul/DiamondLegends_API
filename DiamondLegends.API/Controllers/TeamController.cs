using DiamondLegends.API.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DiamondLegends.API.Mappers;
using DiamondLegends.Domain.Models;
using DiamondLegends.BLL.Interfaces;

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

        [HttpGet("user/{id:int}")]
        public async Task<ActionResult<IEnumerable<TeamViewList>>> GetAllByUser()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<TeamView>> Create([FromBody] TeamCreationForm teamForm)
        { 
            if(teamForm is null || !ModelState.IsValid)
            {
                throw new ArgumentNullException("L'équipe n'est pas valide'");
            }

            int userId = HttpContext.User.FindFirst("Id")?.Value is not null ? int.Parse(HttpContext.User.FindFirst("Id")?.Value) : 0;

            if(userId <= 0)
            {
                throw new ArgumentException("L'utilisateur n'est pas connecté");
            }

            Team team = await _teamService.Create(teamForm.ToTeam(), userId);

            return Ok(team.ToView());
        }
    }
}
