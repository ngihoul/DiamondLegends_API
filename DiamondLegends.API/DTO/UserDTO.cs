using DiamondLegends.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace DiamondLegends.API.DTO
{
    public class UserView
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
    }

    public class UserRegistrationForm
    {
        [Required]
        [MinLength(3)]
        [MaxLength(120)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(250)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public string Password { get; set; }

        [Required]
        [Range(1, 193)]
        public int NationalityId { get; set; }
    }

    public class UserLoginForm
    {
        [Required]
        public string EmailOrUsername { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
