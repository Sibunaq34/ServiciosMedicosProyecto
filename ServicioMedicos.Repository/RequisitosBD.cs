using Dapper;
using ServiciosMedicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository
{
    public class RequisitosBD
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public RequisitosBD(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<RequisitoPuesto>> ListarRequisitos(int idPuesto)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<RequisitoPuesto>(
                    "SP_ListarRequisitos",
                    new
                    {
                        pIdPuesto = idPuesto
                    },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> InsertarRequisito(RequisitoPuesto requisito)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var filas = await connection.ExecuteAsync(
                    "SP_InsertarRequisito",
                    new
                    {
                        pIdPuesto = requisito.IdPuesto,
                        pNombreRequisito = requisito.NombreRequisito
                    },
                    commandType: CommandType.StoredProcedure);

                return filas > 0;
            }
        }

        public async Task<bool> ActualizarRequisito(RequisitoPuesto requisito)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var filas = await connection.ExecuteAsync(
                    "SP_ActualizarRequisito",
                    new
                    {
                        pIdRequisito = requisito.IdRequisito,
                        pNombreRequisito = requisito.NombreRequisito
                    },
                    commandType: CommandType.StoredProcedure);

                return filas > 0;
            }
        }

        public async Task<bool> EliminarRequisito(int idRequisito)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var filas = await connection.ExecuteAsync(
                    "SP_EliminarRequisito",
                    new
                    {
                        pIdRequisito = idRequisito
                    },
                    commandType: CommandType.StoredProcedure);

                return filas > 0;
            }
        }
    }
}
