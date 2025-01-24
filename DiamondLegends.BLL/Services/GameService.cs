using DiamondLegends.BLL.Interfaces;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<List<Game>> GetAll(GameQuery? query = null)
        {
            // QUESTION : a faire en BLL ou API ?
            if (query is not null)
            {
                if (query.LeagueId is not null && query.LeagueId <= 0)
                {
                    throw new ArgumentNullException("L'id de la ligue n'est pas valable");
                }

                if (query.TeamId is not null && query.TeamId <= 0)
                {
                    throw new ArgumentNullException("L'id de l'equipe n'est pas valable");
                }

                if (query.Month is not null && query.Month <= 0 || query.Month > 12)
                {
                    throw new ArgumentNullException("Le mois n'est pas valable");
                }

                if (query.Season is not null && query.Season <= 0)
                {
                    throw new ArgumentNullException("L'annee n'est pas valable");
                }

                if (query.Day is not null && query.Day <= 0 || query.Day > 31)
                {
                    throw new ArgumentNullException("Le jour n'est pas valable");
                }
            }

            List<Game>? games = await _gameRepository.GetAll(query);

            return games;
        }
    }
}
