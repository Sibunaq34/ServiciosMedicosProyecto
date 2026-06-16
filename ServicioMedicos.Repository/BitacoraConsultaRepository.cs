using Dapper;
using ServiciosMedicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository
{
    public class BitacoraConsultaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public BitacoraConsultaRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Bitacora>> ConsultarBitacoras(
            string? usuario,
            string? descripcion,
            int pagina,
            int tamanoPagina)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parametros = new
            {
                pUsuario = usuario,
                pDescripcion = descripcion,
                pPagina = pagina,
                pTamanoPagina = tamanoPagina
            };

            return await connection.QueryAsync<Bitacora>(
                "ConsultarBitacoras",
                parametros,
                commandType: CommandType.StoredProcedure);
        }
    }
}