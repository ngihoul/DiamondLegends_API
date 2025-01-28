using CheckMate.BLL.Services;
using DiamondLegends.BLL.Services.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;
using System.Text.RegularExpressions;

namespace DiamondLegends.BLL.Services
{
    public class UserService : IUserService
    {
        #region Dependencies
        private readonly IUserRepository _userRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly AuthService _authService;
        #endregion

        #region Constructor
        public UserService(IUserRepository userRepository, ICountryRepository countryRepository, AuthService authService)
        {
            _userRepository = userRepository;
            _countryRepository = countryRepository;

            _authService = authService;
        }
        #endregion

        #region Methods
        public async Task<User> Register(User user, int countryId)
        {
            if(user is null)
            {
                throw new ArgumentNullException("L'utilisateur ne peut pas être vide");
            }

            if(countryId <= 0)
            {
                throw new ArgumentOutOfRangeException("La nationalité ne peut pas être vide");
            }
            
            if(await _userRepository.GetByUsername(user.Username) is not null)
            {
                throw new ArgumentException("Le nom d'utilisateur est déjà utilisé");
            }

            if(await _userRepository.GetByEmail(user.Email) is not null)
            {
                throw new ArgumentException("L'email est déjà utilisé");
            }

            user.Salt = _authService.GenerateSalt();
            user.Password = _authService.HashPassword(user.Password, user.Salt);

            Country? nationality = await _countryRepository.GetById(countryId);

            if(nationality is null)
            {
                throw new Exception("Le pays n'existe pas");
            }

            user.Nationality = nationality;

            return await _userRepository.Create(user);
        }

        public async Task<string> Login(string emailOrUsername, string password)
        {
            User? user = null;

            if (isEmail(emailOrUsername))
            {
                user = await _userRepository.GetByEmail(emailOrUsername);
            }
            else
            {
                user = await _userRepository.GetByUsername(emailOrUsername);
            }

            if (user is null || !_authService.Verify(user, password))
            {
                throw new Exception("Données invalides");
            }

            return _authService.GenerateToken(user);
        }

        public async Task<User> GetById(int id)
        {
            User? user = await _userRepository.GetById(id);

            if (user is null)
            {
                throw new ArgumentNullException("L'utilisateur n'existe pas");
            }

            return user;
        }

        private bool isEmail(string email)
        {
            Regex emailRegex = new Regex("^\\S+@\\S+\\.\\S+$");

            return emailRegex.IsMatch(email);
        }
        #endregion
    }
}
