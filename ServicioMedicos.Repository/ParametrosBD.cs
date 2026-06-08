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

        public async Task<IEnumerable<ParametroEntidad>>
            Listar()
        {
            using var connection =
                _dbConnectionFactory.CreateConnection();

            return await connection.QueryAsync<ParametroEntidad>(
                "ListarParametros",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<ParametroEntidad?>
            ObtenerPorId(int id)
        {
            using var connection =
                _dbConnectionFactory.CreateConnection();

            return await connection
                .QuerySingleOrDefaultAsync<ParametroEntidad>(
                    "ObtenerParametroPorId",
                    new
                    {
                        pIdParametro = id
                    },
                    commandType: CommandType.StoredProcedure);
        }

        public async Task<bool>
            Insertar(ParametroEntidad parametro)
        {
            using var connection =
                _dbConnectionFactory.CreateConnection();

            var filas =
                await connection.ExecuteAsync(
                    "InsertarParametro",
                    new
                    {
                        pCodigoParametro =
                            parametro.CodigoParametro,

                        pValor =
                            parametro.Valor
                    },
                    commandType:
                        CommandType.StoredProcedure);

            return filas > 0;
        }

        public async Task<bool>
            Actualizar(ParametroEntidad parametro)
        {
            using var connection =
                _dbConnectionFactory.CreateConnection();

            var filas =
                await connection.ExecuteAsync(
                    "ActualizarParametro",
                    new
                    {
                        pIdParametro =
                            parametro.IdParametro,

                        pCodigoParametro =
                            parametro.CodigoParametro,

                        pValor =
                            parametro.Valor
                    },
                    commandType:
                        CommandType.StoredProcedure);

            return filas > 0;
        }

        public async Task<bool>
            Eliminar(int id)
        {
            using var connection =
                _dbConnectionFactory.CreateConnection();

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
        public async Task<ParametroEntidad?>
            ObtenerPorCodigo(string codigo)
        {
            using var connection =
                _dbConnectionFactory.CreateConnection();

            return await connection
                .QuerySingleOrDefaultAsync<ParametroEntidad>(
                    "ObtenerParametroPorCodigo",
                    new
                    {
                        pCodigoParametro = codigo
                    },
                    commandType: CommandType.StoredProcedure);
        }
    }
}