using Dapper;
using MySql.Data.MySqlClient;
using Servicios_Medicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository
{
    // Persona C - Kenneth: Repositorio Dapper async para OFE2.
    public class ConcursoRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public ConcursoRepository(
            IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IReadOnlyList<Concurso>> ListarAsync(
            int pagina,
            int tamanoPagina,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                var resultado =
                    await connection.QueryAsync<Concurso>(
                        "sp_concursos_listar",
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

        public async Task<Concurso?> ObtenerAsync(
            int idConcurso,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                return await connection.QuerySingleOrDefaultAsync<Concurso>(
                    "sp_concursos_obtener",
                    new
                    {
                        pIdConcurso = idConcurso,
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
            Concurso concurso,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                return await connection.QuerySingleAsync<int>(
                    "sp_concursos_crear",
                    new
                    {
                        pCodigo = concurso.Codigo,
                        pNombre = concurso.Nombre,
                        pFechaInicio = concurso.FechaInicio,
                        pFechaFin = concurso.FechaFin,
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
            Concurso concurso,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                await connection.ExecuteAsync(
                    "sp_concursos_actualizar",
                    new
                    {
                        pIdConcurso = concurso.IdConcurso,
                        pCodigo = concurso.Codigo,
                        pNombre = concurso.Nombre,
                        pFechaInicio = concurso.FechaInicio,
                        pFechaFin = concurso.FechaFin,
                        pEstado = concurso.Estado,
                        pIdUsuario = idUsuario
                    },
                    commandType: CommandType.StoredProcedure);
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public async Task CambiarEstadoAsync(
            int idConcurso,
            string estado,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                await connection.ExecuteAsync(
                    "sp_concursos_cambiar_estado",
                    new
                    {
                        pIdConcurso = idConcurso,
                        pEstado = estado,
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
            int idConcurso,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                await connection.ExecuteAsync(
                    "sp_concursos_eliminar",
                    new
                    {
                        pIdConcurso = idConcurso,
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
