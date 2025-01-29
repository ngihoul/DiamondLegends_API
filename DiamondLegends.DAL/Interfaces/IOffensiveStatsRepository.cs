using DiamondLegends.Domain.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.DAL.Interfaces
{
    public interface IOffensiveStatsRepository
    {
        Task<GameOffensiveStats> Create(GameOffensiveStats stats, SqlConnection? connection = null, SqlTransaction? transaction = null);
    }
}
