using DiamondLegends.BLL.Services.Interfaces;
using DiamondLegends.DAL.Repositories.Interfaces;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.BLL.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<Player> GetById(int id)
        {
            Player? player = await _playerRepository.GetById(id);

            if (player is null)
            {
                throw new ArgumentException("Le joueur n'existe pas");
            }

            return player;
        }
    }
}
