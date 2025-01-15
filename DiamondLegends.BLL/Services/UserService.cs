using DiamondLegends.BLL.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICountryRepository _countryRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Create(User user, int countryId)
        {
            user.Nationality = await _countryRepository.GetById(countryId);
            return await _userRepository.Create(user);
        }
    }
}
