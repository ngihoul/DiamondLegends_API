using DiamondLegends.API.DTO;
using DiamondLegends.API.Mappers;
using DiamondLegends.BLL.Interfaces;
using DiamondLegends.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLegends.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("auth/register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserView>> Register([FromBody] UserRegistrationForm userForm)
        {
            if(userForm is null || !ModelState.IsValid)
            {
                throw new ArgumentNullException("Données invalides");
            }

            User user = await _userService.Create(userForm.ToUser(), userForm.NationalityId);

            return Ok(user.ToView());
        }
    }
}
