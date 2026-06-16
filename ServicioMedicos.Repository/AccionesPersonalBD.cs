using Dapper;
using ServiciosMedicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository
{
    public class AccionesPersonalBD
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public AccionesPersonalBD(
            IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<AccionPersonal>>
            ListarAcciones()
        {
            using var connection =
                _dbConnectionFactory.CreateConnection();

            return await connection.QueryAsync<AccionPersonal>(
                "SP_ListarAccionesPersonal",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool>
            InsertarAccion(
            AccionPersonal accion)
        {
            using var connection =
                _dbConnectionFactory.CreateConnection();

            var filas =
                await connection.ExecuteAsync(
                    "SP_InsertarAccionPersonal",
                    new
                    {
                        pFecha = accion.FechaAccion,
                        pDescripcion = accion.Descripcion,
                        pIdEmpleado = accion.IdEmpleado,
                        pIdJefatura = accion.IdJefatura
                    },
                    commandType:
                    CommandType.StoredProcedure);

            return filas > 0;
        }
    }
}