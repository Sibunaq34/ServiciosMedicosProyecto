using Dapper;
using ServiciosMedicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository
{
    public class BitacoraBD
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public BitacoraBD(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<BitacoraEntidad>> ConsultarBitacoras(
            string? usuario,
            string? descripcion)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parametros = new
            {
                pUsuario = usuario,
                pDescripcion = descripcion
            };

            return await connection.QueryAsync<BitacoraEntidad>(
                "ConsultarBitacoras",
                parametros,
                commandType: CommandType.StoredProcedure);
        }
    }
}