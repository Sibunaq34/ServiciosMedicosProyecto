using Dapper;
using MySql.Data.MySqlClient;
using Servicios_Medicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository
{
    // Persona C - Kenneth: Repositorio Dapper async para OFE3.
    public class PreparacionAcademicaRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public PreparacionAcademicaRepository(
            IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IReadOnlyList<PreparacionAcademica>> ListarPorOferenteAsync(
            int idOferente,
            int pagina,
            int tamanoPagina,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                var resultado =
                    await connection.QueryAsync<PreparacionAcademica>(
                        "sp_preparacion_listar_por_oferente",
                        new
                        {
                            pIdOferente = idOferente,
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

        public async Task<PreparacionAcademica?> ObtenerAsync(
            int idPreparacion,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                return await connection.QuerySingleOrDefaultAsync<PreparacionAcademica>(
                    "sp_preparacion_obtener",
                    new
                    {
                        pIdPreparacion = idPreparacion,
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
            PreparacionAcademica preparacion,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                return await connection.QuerySingleAsync<int>(
                    "sp_preparacion_crear",
                    new
                    {
                        pIdOferente = preparacion.IdOferente,
                        pIdInstitucion = preparacion.IdInstitucion,
                        pTitulo = preparacion.Titulo,
                        pFechaInicio = preparacion.FechaInicio,
                        pFechaFin = preparacion.FechaFin,
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
            PreparacionAcademica preparacion,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                await connection.ExecuteAsync(
                    "sp_preparacion_actualizar",
                    new
                    {
                        pIdPreparacion = preparacion.IdPreparacion,
                        pIdInstitucion = preparacion.IdInstitucion,
                        pTitulo = preparacion.Titulo,
                        pFechaInicio = preparacion.FechaInicio,
                        pFechaFin = preparacion.FechaFin,
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
            int idPreparacion,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                await connection.ExecuteAsync(
                    "sp_preparacion_eliminar",
                    new
                    {
                        pIdPreparacion = idPreparacion,
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
