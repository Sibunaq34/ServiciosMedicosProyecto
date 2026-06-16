using Dapper;
using ServiciosMedicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository
{
    public class EmpleadosRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public EmpleadosRepository(
            IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<OferenteCombo>>
            ListarOferentes()
        {
            using var connection =
                _dbConnectionFactory.CreateConnection();

            return await connection.QueryAsync<OferenteCombo>(
                "SP_ListarOferentes",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Puesto>>
            ListarPuestos()
        {
            using var connection =
                _dbConnectionFactory.CreateConnection();

            return await connection.QueryAsync<Puesto>(
                "SP_ListarPuestos",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EmpleadoCombo>>
            ListarEmpleados()
        {
            using var connection =
                _dbConnectionFactory.CreateConnection();

            return await connection.QueryAsync<EmpleadoCombo>(
                "SP_ListarEmpleados",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool>
            ContratarEmpleado(
            EmpleadoContratacion empleado)
        {
            using var connection =
                _dbConnectionFactory.CreateConnection();

            var filas =
                await connection.ExecuteAsync(
                    "SP_ContratarEmpleado",
                    new
                    {
                        pIdOferente = empleado.IdOferente,
                        pIdPuesto = empleado.IdPuesto,
                        pIdJefatura = empleado.IdJefatura
                    },
                    commandType:
                    CommandType.StoredProcedure);

            return filas > 0;
        }
    }
}