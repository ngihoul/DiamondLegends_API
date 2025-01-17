using DiamondLegends.API.DTO;
using DiamondLegends.API.Mappers;
using DiamondLegends.BLL.Interfaces;
using DiamondLegends.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLegends.API.Controllers
{
    [Route("api")]
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

            User user = await _userService.Register(userForm.ToUser(), userForm.NationalityId);

            return Ok(user.ToView());
        }

        [HttpPost("auth/login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Login([FromBody] UserLoginForm userForm)
        {
            if (userForm is null || !ModelState.IsValid)
            {
                throw new ArgumentNullException("Données invalides");
            }

            string token = await _userService.Login(userForm.EmailOrUsername, userForm.Password);

            if(token is null)
            {
                throw new ArgumentNullException("Données invalides");
            }

            return Ok(token);
        }
    }
}
