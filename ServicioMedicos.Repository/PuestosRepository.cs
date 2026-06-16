using Dapper;
using ServiciosMedicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository
{
    public class PuestosRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public PuestosRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Puesto>> ListarPuestos()
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Puesto>(
                    "SP_ListarPuestos",
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Puesto?> ObtenerPuesto(int idPuesto)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            return await connection.QuerySingleOrDefaultAsync<Puesto>(
                "SP_ObtenerPuesto",
                new
                {
                    pIdPuesto = idPuesto
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> InsertarPuesto(Puesto puesto)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var filas = await connection.ExecuteAsync(
                    "SP_InsertarPuesto",
                    new
                    {
                        pCodigo = puesto.CodigoPuesto,
                        pNombre = puesto.NombrePuesto,
                        pSalario = puesto.MontoSalario,
                        pIdJefatura = puesto.IdPuestoJefac
                    },
                    commandType: CommandType.StoredProcedure);

                return filas > 0;
            }
        }

        public async Task<bool> ActualizarPuesto(Puesto puesto)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var filas = await connection.ExecuteAsync(
                    "SP_ActualizarPuesto",
                    new
                    {
                        pIdPuesto = puesto.IdPuesto,
                        pCodigo = puesto.CodigoPuesto,
                        pNombre = puesto.NombrePuesto,
                        pSalario = puesto.MontoSalario,
                        pIdJefatura = puesto.IdPuestoJefac
                    },
                    commandType: CommandType.StoredProcedure);

                return filas > 0;
            }
        }

        public async Task<bool> EliminarPuesto(int idPuesto)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var filas = await connection.ExecuteAsync(
                    "SP_EliminarPuesto",
                    new
                    {
                        pIdPuesto = idPuesto
                    },
                    commandType: CommandType.StoredProcedure);

                return filas > 0;
            }
        }
    }
}
