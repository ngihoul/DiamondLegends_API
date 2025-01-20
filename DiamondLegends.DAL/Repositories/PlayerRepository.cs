﻿using Dapper;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.DAL.Factories.Interfaces;
using DiamondLegends.Domain.Enums;
using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;

namespace DiamondLegends.DAL.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PlayerRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Player> Create(Player player, int teamId)
        {
            using (var connection = _connectionFactory.Create())
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
                                    "INSERT INTO MM_Players_Positions (Player, Position) VALUES (@Player, @Position)",
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
    }
}