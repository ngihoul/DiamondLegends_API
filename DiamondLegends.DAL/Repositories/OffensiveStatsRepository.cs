using Dapper;
using DiamondLegends.DAL.Factories.Interfaces;
using DiamondLegends.DAL.Repositories.Interfaces;
using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;

namespace DiamondLegends.DAL.Repositories
{
    public class OffensiveStatsRepository : IOffensiveStatsRepository
    {
        private readonly IDbConnectionFactory _connection;

        public OffensiveStatsRepository(IDbConnectionFactory connection)
        {
            _connection = connection;
        }
        public async Task<GameOffensiveStats> Create(GameOffensiveStats stats, SqlConnection? conn = null, SqlTransaction? transaction = null)
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
                    INSERT INTO [Game_Offensive_Stats]
                    ([Game], Player, [Order], Position, PA, AB, R, H, [2B], [3B], HR, RBI, BB, IBB, SO, SB, CS)
                    OUTPUT Inserted.Id
                    VALUES
                    (@Game, @Player, @Order, @Position, @PA, @AB, @R, @H, @Double, @Triple, @HR, @RBI, @BB, @IBB, @SO, @SB, @CS)";

                command.Parameters.AddWithValue("@Game", stats.Game.Id);
                command.Parameters.AddWithValue("@Player", stats.Player.Id);
                command.Parameters.AddWithValue("@Order", stats.Order);
                command.Parameters.AddWithValue("@Position", stats.Position);
                command.Parameters.AddWithValue("@PA", stats.PA);
                command.Parameters.AddWithValue("@AB", stats.AB);
                command.Parameters.AddWithValue("@R", stats.R);
                command.Parameters.AddWithValue("@H", stats.H);
                command.Parameters.AddWithValue("@Double", stats.Double);
                command.Parameters.AddWithValue("@Triple", stats.Triple);
                command.Parameters.AddWithValue("@HR", stats.HR);
                command.Parameters.AddWithValue("@RBI", stats.RBI);
                command.Parameters.AddWithValue("@BB", stats.BB);
                command.Parameters.AddWithValue("@IBB", stats.IBB);
                command.Parameters.AddWithValue("@SO", stats.SO);
                command.Parameters.AddWithValue("@SB", stats.SB);
                command.Parameters.AddWithValue("@CS", stats.CS);

                stats.Id = (int) await command.ExecuteScalarAsync();

                if (stats.Id <= 0)
                {
                    throw new Exception("Erreur lors de l'ajout d'une stat offensive");
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
