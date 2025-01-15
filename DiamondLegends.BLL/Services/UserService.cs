using CheckMate.BLL.Services;
using DiamondLegends.BLL.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICountryRepository _countryRepository;

        private readonly AuthService _authService;

        public UserService(IUserRepository userRepository, ICountryRepository countryRepository, AuthService authService)
        {
            _userRepository = userRepository;
            _countryRepository = countryRepository;

            _authService = authService;
        }

        public async Task<User> Create(User user, int countryId)
        {
            if(user is null)
            {
                throw new ArgumentNullException("L'utilisateur ne peut pas être vide");
            }

            if(countryId <= 0)
            {
                throw new ArgumentOutOfRangeException("La nationalité ne peut pas être vide");
            }

            user.Salt = _authService.GenerateSalt();
            user.Password = _authService.HashPassword(user.Password, user.Salt);

            user.Nationality = await _countryRepository.GetById(countryId);

            return await _userRepository.Create(user);
        }
    }
}
