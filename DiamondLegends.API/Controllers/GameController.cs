using DiamondLegends.API.DTO;
using DiamondLegends.API.Mappers;
using DiamondLegends.BLL.Services.Interfaces;
using DiamondLegends.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLegends.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController(IGameService _gameService) : ControllerBase
    {
        [HttpGet("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GameView>> Get([FromRoute] int id)
        {
            if(id <= 0)
            {
                throw new ArgumentNullException("L'id n'est pas valable.");
            }

            Game game = await _gameService.GetById(id);

            return Ok(game.ToView());
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GameViewList>>> GetAll([FromQuery] GameQuery query)
        {
            List<Game> games = await _gameService.GetAll(query);

            return Ok(games.Select(g => g.ToViewList()));
        }

        [HttpPost("play/{id:int}")]
        [Authorize]
        public async Task<ActionResult<GameView>> Play([FromRoute] int id, [FromBody] GameLineUp lineUp)
        {
            if(lineUp is null || !ModelState.IsValid)
            {
                throw new ArgumentNullException("Données invalides.");
            }

            List<GameOffensiveStats> offensiveLineUp = lineUp.ToOffensiveStatList();
            GamePitchingStats startingPitcher = lineUp.ToPitchingStat();

            Game game = await _gameService.Play(id, offensiveLineUp, startingPitcher);

            return Ok(game.ToView());
        }
    }
}
