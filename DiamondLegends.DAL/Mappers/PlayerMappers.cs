using DiamondLegends.Domain.Enums;
using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.DAL.Mappers
{
    public static class PlayerMappers
    {
        public static Player FullPlayer(SqlDataReader reader, List<Position> positions)
        {
            return new Player()
            {
                Id = (int)reader["Id"],
                Firstname = (string)reader["FirstName"],
                Lastname = (string)reader["LastName"],
                DateOfBirth = (DateTime)reader["Date_of_birth"],
                Nationality = new Country()
                {
                    Id = (int)reader["NationalityId"],
                    Name = (string)reader["NationalityName"],
                    Alpha2 = (string)reader["NationalityAlpha2"]
                },
                Positions = positions,
                Throw = (int)reader["Throw"],
                Bat = (int)reader["Bat"],
                Salary = Convert.ToDecimal(reader["Salary"]),
                Energy = (int)reader["Energy"],
                Contact = (int)reader["Contact"],
                ContactPotential = (int)reader["Contact_potential"],
                Power = (int)reader["Power"],
                PowerPotential = (int)reader["Power_potential"],
                Running = (int)reader["Running"],
                RunningPotential = (int)reader["Running_potential"],
                Defense = (int)reader["Defense"],
                DefensePotential = (int)reader["Defense_potential"],
                Mental = (int)reader["Mental"],
                MentalPotential = (int)reader["Mental_potential"],
                Stamina = (int)reader["Stamina"],
                StaminaPotential = (int)reader["Stamina_potential"],
                Control = (int)reader["Control"],
                ControlPotential = (int)reader["Control_potential"],
                Velocity = (int)reader["Velocity"],
                VelocityPotential = (int)reader["Velocity_potential"],
                Movement = (int)reader["Movement"],
                MovementPotential = (int)reader["Movement_potential"]
            };
        }
    }
}
