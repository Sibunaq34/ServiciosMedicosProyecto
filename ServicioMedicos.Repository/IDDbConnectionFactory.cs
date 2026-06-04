using System.Data;

namespace Servicios_Medicos.Repository
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}