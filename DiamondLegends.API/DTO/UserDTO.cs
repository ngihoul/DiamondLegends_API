using DiamondLegends.Domain.Models;

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
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public int NationalityId { get; set; }
    }
}
