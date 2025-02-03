using Dapper;
using DiamondLegends.DAL.Factories.Interfaces;
using DiamondLegends.Domain.Enums;
using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;
using DiamondLegends.DAL.Mappers;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using DiamondLegends.DAL.Repositories.Interfaces;

namespace DiamondLegends.DAL.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IDbConnectionFactory _connection;

        public PlayerRepository(IDbConnectionFactory connection)
        {
            _connection = connection;
        }

        public async Task<Player> Create(Player player, int teamId)
        {
            using (var connection = _connection.Create())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        player.Id = await connection.QuerySingleAsync<int>(
                            "INSERT INTO Players(Firstname, Lastname, Date_of_birth, Nationality, [Throw], Bat, Salary, Energy, Contact, Contact_Potential, [Power], Power_Potential, Running, Running_Potential, Defense, Defense_Potential, Mental, Mental_Potential, Stamina, Stamina_potential, [Control], Control_potential, Velocity, Velocity_potential, Movement, Movement_potential) " +
                            "OUTPUT INSERTED.Id " +
                            "VALUES (@Firstname, @Lastname, @DateOfBirth, @Nationality, @Throw, @Bat, @Salary, @Energy, @Contact, @Contact_Potential, @Power, @Power_Potential, @Running, @Running_Potential, @Defense, @Defense_Potential, @Mental, @Mental_Potential, @Stamina, @Stamina_potential, @Control, @Control_potential, @Velocity, @Velocity_potential, @Movement, @Movement_potential)",
                            new
                            {
                                player.Firstname,
                                player.Lastname,
                                player.DateOfBirth,
                                Nationality = player.Nationality.Id,
                                player.Throw,
                                player.Bat,
                                player.Salary,
                                player.Energy,
                                player.Contact,
                                Contact_Potential = player.ContactPotential,
                                player.Power,
                                Power_Potential = player.PowerPotential,
                                player.Running,
                                Running_Potential = player.RunningPotential,
                                player.Defense,
                                Defense_Potential = player.DefensePotential,
                                player.Mental,
                                Mental_Potential = player.MentalPotential,
                                player.Stamina,
                                Stamina_potential = player.StaminaPotential,
                                player.Control,
                                Control_potential = player.ControlPotential,
                                player.Velocity,
                                Velocity_potential = player.VelocityPotential,
                                player.Movement,
                                Movement_potential = player.MovementPotential
                            },
                            transaction
                        );

                        if (player.Positions != null && player.Positions.Any())
                        {
                            foreach (Position position in player.Positions)
                            {
                                await connection.ExecuteAsync(
                                    "INSERT INTO Positions (Player, Position) VALUES (@Player, @Position)",
                                    new { Player = player.Id, Position = position },
                                    transaction
                                );
                            }
                        }

                        await connection.ExecuteAsync(
                            "INSERT INTO Rosters (Player, Team, AddedAt) VALUES (@Player, @Team, GETDATE())",
                            new { Player = player.Id, Team = teamId },
                            transaction
                        );

                        await transaction.CommitAsync();
                        return player;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception("Erreur lors de la création du joueur", ex);
                    }
                }
            }
        }

        public async Task<Player?> GetById(int id)
        {
            using(var connection = _connection.Create())
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = "SELECT P.Id, P.Firstname, P.Lastname, P.Date_of_birth, C.Id AS NationalityId, C.Name AS NationalityName, C.Alpha2 AS NationalityAlpha2, P.[Throw], P.Bat, P.Salary, P.Energy, P.Contact, P.Contact_Potential, P.[Power], P.Power_Potential, P.Running, P.Running_Potential, P.Defense, P.Defense_Potential, P.Mental, P.Mental_Potential, P.Stamina, P.Stamina_potential, P.[Control], P.Control_potential, P.Velocity, P.Velocity_potential, P.Movement, P.Movement_potential, T.Id AS TeamId, T.Name AS TeamName, T.Abbreviation AS TeamAbbreviation " +
                    "FROM Players AS P " +
                    "JOIN Countries AS C ON P.Nationality = C.Id " +
                    "JOIN Rosters AS R ON P.Id = R.Player " +
                    "JOIN Teams AS T ON R.Team = T.Id " +
                    "WHERE P.Id = @Id";

                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();

                SqlDataReader reader = await command.ExecuteReaderAsync();
                Player player = null;

                if(await reader.ReadAsync())
                {
                    List<Position> positions = await GetPositionsForPlayer(id);
                    player = PlayerMappers.FullPlayer(reader, positions);
                }

                return player;
            }
        }

        public async Task<List<Player>> GetAllByTeam(int teamId)
        {
            using(var connection = _connection.Create())
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT P.Id, P.Firstname, P.Lastname, P.Date_of_birth, C.Id AS NationalityId, C.Name AS NationalityName, C.Alpha2 AS NationalityAlpha2, P.[Throw], P.Bat, P.Salary, P.Energy, ROUND(AVG(((CAST(OS.H AS DECIMAL) + CAST(OS.[2B] AS DECIMAL) + CAST(OS.[3B] AS DECIMAL) + CAST(OS.HR AS DECIMAL)) / CAST(OS.AB AS DECIMAL))), 3) AS [AVG] " +
                    "FROM Players AS P " +
                    "JOIN Countries AS C ON P.Nationality = C.Id " +
                    "JOIN Rosters AS R ON P.Id = R.Player " +
                    "JOIN Teams AS T ON R.Team = T.Id " + 
                    "LEFT JOIN[Game_Offensive_Stats]  AS OS ON P.Id = OS.Player " +
                    "WHERE R.Team = @TeamId " +
                    "GROUP BY P.Id, P.Firstname, P.Lastname, P.Date_of_birth, C.Id, C.Name, C.Alpha2, P.[Throw], P.Bat, P.Salary, P.Energy";
                command.Parameters.AddWithValue("@TeamId", teamId);
                await connection.OpenAsync();
                List<Player> players = new List<Player>();
                using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {
                    List<Position> positions = await GetPositionsForPlayer((int)reader["Id"]);
                    players.Add(PlayerMappers.ForViewList(reader, positions));
                }

                return players;
            }
        }

        public async Task<List<Position>> GetPositionsForPlayer(int playerId)
        {
            using (var connection = _connection.Create())
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = "SELECT Position FROM Positions WHERE Player = @PlayerId";

                command.Parameters.AddWithValue("@PlayerId", playerId);

                await connection.OpenAsync();

                List<Position> positions = new List<Position>();

                using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    positions.Add((Position)reader["Position"]);
                }

                return positions;
            }
        }
    }
}