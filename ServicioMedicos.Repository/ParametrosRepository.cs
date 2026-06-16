using Dapper;
using Servicios_Medicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository
{
    public class ParametroBD
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public ParametroBD(
            IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Parametros>>Listar(
            int pagina,
            int tamanoPagina)
        {
            using var connection =_dbConnectionFactory.CreateConnection();

            var parametros = new
            {
                pPagina = pagina,
                pTamanoPagina = tamanoPagina
            };

            return await connection.QueryAsync<Parametros>("ListarParametros", parametros, commandType: CommandType.StoredProcedure);
        }


        public async Task<Parametros?>ObtenerPorId(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            return await connection.QuerySingleOrDefaultAsync<Parametros>(
                    "ObtenerParametroPorId",
                    new
                    {
                        pIdParametro = id
                    },
                    commandType: CommandType.StoredProcedure);
        }


        public async Task<bool>Insertar(Parametros parametro)
        {
            using var connection =
                _dbConnectionFactory.CreateConnection();

            var filas = await connection.ExecuteAsync( "InsertarParametro",
                    new
                    {
                        pCodigoParametro = parametro.CodigoParametro,

                        pValor =parametro.Valor
                    },
                    commandType:
                        CommandType.StoredProcedure);

            return filas > 0;
        }

        public async Task<bool>Actualizar(Parametros parametro)
        {
            using var connection =
                _dbConnectionFactory.CreateConnection();

            var filas = await connection.ExecuteAsync("ActualizarParametro",new{

                        pIdParametro = parametro.IdParametro,
                        pCodigoParametro =parametro.CodigoParametro,
                        pValor = parametro.Valor

                    },commandType:CommandType.StoredProcedure);

            return filas > 0;
        }

        public async Task<bool>Eliminar(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var filas =
                await connection.ExecuteAsync(
                    "EliminarParametro",
                    new
                    {
                        pIdParametro = id
                    },
                    commandType:
                        CommandType.StoredProcedure);

            return filas > 0;
        }
        public async Task<Parametros?> ObtenerPorCodigo(string codigo)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            return await connection
                .QuerySingleOrDefaultAsync<Parametros>(
                    "ObtenerParametroPorCodigo",
                    new
                    {
                        pCodigoParametro = codigo
                    },
                    commandType: CommandType.StoredProcedure);
        }
    }
}