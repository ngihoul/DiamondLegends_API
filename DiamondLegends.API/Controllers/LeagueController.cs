﻿using DiamondLegends.API.DTO;
using DiamondLegends.API.Extensions;
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
            if (id <= 0)
            {
                // TODO : harmoniser message d'erreur - CustomException ?!
                throw new ArgumentNullException("L'id n'est pas valable.");
            }

            League league = await _leagueService.GetById(id);

            return Ok(league.ToView());
        }

        [HttpGet("{leagueId:int}/next-day/{teamId:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LeagueView>> NextDay([FromRoute] int leagueId, [FromRoute] int teamId)
        {
            if(leagueId <= 0)
            {
                throw new ArgumentNullException("L'id n'est pas valable.");
            }

            if(teamId <= 0)
            {
                throw new ArgumentNullException("L'id n'est pas valable.");
            }

            League league = await _leagueService.NextDay(leagueId, teamId);

            return Ok(league.ToView());
        }

        [HttpGet("{leagueId:int}/next-game/{teamId:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LeagueView>> NextGame([FromRoute] int leagueId, [FromRoute] int teamId)
        {
            if (leagueId <= 0)
            {
                throw new ArgumentNullException("L'id n'est pas valable.");
            }

            if (teamId <= 0)
            {
                throw new ArgumentNullException("L'id n'est pas valable.");
            }

            League league = await _leagueService.NextGame(leagueId, teamId);

            return Ok(league.ToView());
        }
    }
}
