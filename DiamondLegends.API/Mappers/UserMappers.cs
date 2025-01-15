using DiamondLegends.API.DTO;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.API.Mappers
{
    public static class UserMappers
    {
        public static User ToUser(this UserRegistrationForm userForm)
        {
            return new User()
            {
                Username = userForm.Username,
                Email = userForm.Email,
                Password = userForm.Password
            };
        }

        public static UserView ToView(this User user)
        {
            return new UserView()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Nationality = user.Nationality.Name
            };
        }
    }
}
