using Microsoft.Data.SqlClient;

namespace DiamondLegends.DAL.Factories.Interfaces
{
    public interface IDbConnectionFactory
    {
        SqlConnection Create();
    }
}
