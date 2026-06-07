using Dapper;
using MySql.Data.MySqlClient;
using Servicios_Medicos.Entities;
using System.Data;
using System.Text.Json;

namespace Servicios_Medicos.Repository
{
    // Persona C - Kenneth: Repositorio Dapper async para OFE1.
    public class OferenteRepository
    {
        private static readonly JsonSerializerOptions JsonOptions =
            new()
            {
                PropertyNameCaseInsensitive = true
            };

        private readonly IDbConnectionFactory _dbConnectionFactory;

        public OferenteRepository(
            IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IReadOnlyList<OferenteListado>> ListarAsync(
            int pagina,
            int tamanoPagina,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                var resultado =
                    await connection.QueryAsync<OferenteRaw>(
                        "sp_oferentes_listar",
                        new
                        {
                            pPagina = pagina,
                            pTamanoPagina = tamanoPagina,
                            pIdUsuario = idUsuario
                        },
                        commandType: CommandType.StoredProcedure);

                return resultado
                    .Select(MapearListado)
                    .ToList();
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public async Task<OferenteDetalle?> ObtenerAsync(
            int idOferente,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                var raw =
                    await connection.QuerySingleOrDefaultAsync<OferenteRaw>(
                        "sp_oferentes_obtener",
                        new
                        {
                            pIdOferente = idOferente,
                            pIdUsuario = idUsuario
                        },
                        commandType: CommandType.StoredProcedure);

                return raw == null
                    ? null
                    : MapearDetalle(raw);
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public async Task<int> CrearAsync(
            Oferente oferente,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                return await connection.QuerySingleAsync<int>(
                    "sp_oferentes_crear",
                    new
                    {
                        pIdentificacion = oferente.Identificacion,
                        pTipoIdentificacion = oferente.TipoIdentificacion,
                        pNombreCompleto = oferente.NombreCompleto,
                        pFechaNacimiento = oferente.FechaNacimiento,
                        pCorreosJson = JsonSerializer.Serialize(oferente.Correos),
                        pTelefonosJson = JsonSerializer.Serialize(oferente.Telefonos),
                        pConcursosJson = JsonSerializer.Serialize(oferente.ConcursosIds),
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
            Oferente oferente,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                await connection.ExecuteAsync(
                    "sp_oferentes_actualizar",
                    new
                    {
                        pIdOferente = oferente.IdOferente,
                        pIdentificacion = oferente.Identificacion,
                        pTipoIdentificacion = oferente.TipoIdentificacion,
                        pNombreCompleto = oferente.NombreCompleto,
                        pFechaNacimiento = oferente.FechaNacimiento,
                        pCorreosJson = JsonSerializer.Serialize(oferente.Correos),
                        pTelefonosJson = JsonSerializer.Serialize(oferente.Telefonos),
                        pConcursosJson = JsonSerializer.Serialize(oferente.ConcursosIds),
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
            int idOferente,
            int idUsuario)
        {
            try
            {
                using var connection =
                    _dbConnectionFactory.CreateConnection();

                await connection.ExecuteAsync(
                    "sp_oferentes_eliminar",
                    new
                    {
                        pIdOferente = idOferente,
                        pIdUsuario = idUsuario
                    },
                    commandType: CommandType.StoredProcedure);
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        private static OferenteListado MapearListado(
            OferenteRaw raw)
        {
            return new OferenteListado
            {
                IdOferente = raw.IdOferente,
                IdPersona = raw.IdPersona,
                Identificacion = raw.Identificacion,
                TipoIdentificacion = raw.TipoIdentificacion,
                NombreCompleto = raw.NombreCompleto,
                FechaNacimiento = raw.FechaNacimiento,
                FechaRegistro = raw.FechaRegistro,
                Correos = DeserializarLista<string>(raw.Correos),
                Telefonos = DeserializarLista<string>(raw.Telefonos),
                Concursos = DeserializarLista<Concurso>(raw.Concursos)
            };
        }

        private static OferenteDetalle MapearDetalle(
            OferenteRaw raw)
        {
            var concursos =
                DeserializarLista<Concurso>(raw.Concursos);

            return new OferenteDetalle
            {
                IdOferente = raw.IdOferente,
                IdPersona = raw.IdPersona,
                Identificacion = raw.Identificacion,
                TipoIdentificacion = raw.TipoIdentificacion,
                NombreCompleto = raw.NombreCompleto,
                FechaNacimiento = raw.FechaNacimiento,
                FechaRegistro = raw.FechaRegistro,
                Correos = DeserializarLista<string>(raw.Correos).ToList(),
                Telefonos = DeserializarLista<string>(raw.Telefonos).ToList(),
                ConcursosIds = concursos
                    .Select(concurso => concurso.IdConcurso)
                    .ToList(),
                Concursos = concursos
            };
        }

        private static IReadOnlyList<T> DeserializarLista<T>(
            string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return Array.Empty<T>();

            return JsonSerializer.Deserialize<List<T>>(
                json,
                JsonOptions) ?? new List<T>();
        }

        private sealed class OferenteRaw
        {
            public int IdOferente { get; set; }

            public int IdPersona { get; set; }

            public string Identificacion { get; set; } = string.Empty;

            public string TipoIdentificacion { get; set; } = string.Empty;

            public string NombreCompleto { get; set; } = string.Empty;

            public DateTime FechaNacimiento { get; set; }

            public DateTime FechaRegistro { get; set; }

            public string? Correos { get; set; }

            public string? Telefonos { get; set; }

            public string? Concursos { get; set; }
        }
    }
}
