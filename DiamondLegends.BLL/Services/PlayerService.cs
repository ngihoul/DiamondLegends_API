using DiamondLegends.BLL.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if(id <= 0)
            {
                throw new ArgumentNullException("L'id ne peut pas être nul");
            }

            Player? player = await _playerRepository.GetById(id);

            if (player is null)
            {
                throw new ArgumentException("Le joueur n'existe pas");
            }

            return player;
        }
    }
}
