using Dapper;
using DiamondLegends.DAL.Factories.Interfaces;
using DiamondLegends.DAL.Repositories.Interfaces;
using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;

namespace DiamondLegends.DAL.Repositories
{
    public class PitchingStatsRepository : IPitchingStatsRepository
    {
        private readonly IDbConnectionFactory _connection;

        public PitchingStatsRepository(IDbConnectionFactory connection)
        {
            _connection = connection;
        }

        public async Task<GamePitchingStats> Create(GamePitchingStats stats, SqlConnection? conn = null, SqlTransaction? transaction = null)
        {
            var connection = conn ?? _connection.Create();

            try
            {
                if (conn is null)
                {
                    await connection.OpenAsync();
                }

                using var command = connection.CreateCommand();

                if (transaction is not null)
                {
                    command.Transaction = transaction;
                }

                command.CommandText = @"
                    INSERT INTO [Game_Pitching_Stats]
                    ([Game], Player, W, L, G, GS, CG, SHO, HLD, SV, SVO, IP, H, R, ER, HR, NP, BB, IBB, SO)
                    OUTPUT Inserted.Id
                    VALUES
                    (@Game, @Player, @W, @L, @G, @GS, @CG, @SHO, @HLD, @SV, @SVO, @IP, @H, @R, @ER, @HR, @NP, @BB, @IBB, @SO)";

                command.Parameters.AddWithValue("@Game", stats.Game.Id);
                command.Parameters.AddWithValue("@Player", stats.Player.Id);
                command.Parameters.AddWithValue("@W", stats.W);
                command.Parameters.AddWithValue("@L", stats.L);
                command.Parameters.AddWithValue("@G", stats.G);
                command.Parameters.AddWithValue("@GS", stats.GS);
                command.Parameters.AddWithValue("@CG", stats.CG);
                command.Parameters.AddWithValue("@SHO", stats.SHO);
                command.Parameters.AddWithValue("@HLD", stats.HLD);
                command.Parameters.AddWithValue("@SV", stats.SV);
                command.Parameters.AddWithValue("@SVO", stats.SVO);
                command.Parameters.AddWithValue("@IP", stats.IP);
                command.Parameters.AddWithValue("@H", stats.H);
                command.Parameters.AddWithValue("@R", stats.R);
                command.Parameters.AddWithValue("@ER", stats.ER);
                command.Parameters.AddWithValue("@HR", stats.HR);
                command.Parameters.AddWithValue("@NP", stats.NP);
                command.Parameters.AddWithValue("@BB", stats.BB);
                command.Parameters.AddWithValue("@IBB", stats.IBB);
                command.Parameters.AddWithValue("@SO", stats.SO);

                stats.Id = (int) await command.ExecuteScalarAsync();

                if (stats.Id <= 0)
                {
                    throw new Exception("Erreur lors de l'ajout d'une stat pitching");
                }

                return stats;
            }
            finally
            {
                if (conn is null)
                {
                    await connection.CloseAsync();
                }
            }
        }
    }
}
