using Dapper;
using MySql.Data.MySqlClient;
using Servicios_Medicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository
{
    // Persona C - Kenneth: Repositorio Dapper async para GEN5.
    public class InstitucionEducativaRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public InstitucionEducativaRepository(
            IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IReadOnlyList<InstitucionEducativa>> ListarAsync(
            int pagina,
            int tamanoPagina,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                var resultado =
                    await connection.QueryAsync<InstitucionEducativa>(
                        "sp_instituciones_listar",
                        new
                        {
                            pPagina = pagina,
                            pTamanoPagina = tamanoPagina,
                            pIdUsuario = idUsuario
                        },
                        commandType: CommandType.StoredProcedure);

                return resultado.ToList();
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public async Task<InstitucionEducativa?> ObtenerAsync(
            int idInstitucion,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                return await connection.QuerySingleOrDefaultAsync<InstitucionEducativa>(
                    "sp_instituciones_obtener",
                    new
                    {
                        pIdInstitucion = idInstitucion,
                        pIdUsuario = idUsuario
                    },
                    commandType: CommandType.StoredProcedure);
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public async Task<int> CrearAsync(
            InstitucionEducativa institucion,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                return await connection.QuerySingleAsync<int>(
                    "sp_instituciones_crear",
                    new
                    {
                        pCodigo = institucion.Codigo,
                        pNombre = institucion.Nombre,
                        pIdUsuario = idUsuario
                    },
                    commandType: CommandType.StoredProcedure);
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public async Task ActualizarAsync(
            InstitucionEducativa institucion,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                await connection.ExecuteAsync(
                    "sp_instituciones_actualizar",
                    new
                    {
                        pIdInstitucion = institucion.IdInstitucion,
                        pCodigo = institucion.Codigo,
                        pNombre = institucion.Nombre,
                        pIdUsuario = idUsuario
                    },
                    commandType: CommandType.StoredProcedure);
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public async Task EliminarAsync(
            int idInstitucion,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                await connection.ExecuteAsync(
                    "sp_instituciones_eliminar",
                    new
                    {
                        pIdInstitucion = idInstitucion,
                        pIdUsuario = idUsuario
                    },
                    commandType: CommandType.StoredProcedure);
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }
    }
}
