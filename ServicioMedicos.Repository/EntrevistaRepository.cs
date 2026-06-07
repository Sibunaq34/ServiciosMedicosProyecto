using Dapper;
using Servicios_Medicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository
{
    public class EntrevistaRepository
    {
        private readonly IDbConnectionFactory _db;

        public EntrevistaRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<Entrevista> Items, int Total)> ObtenerPaginado(int pagina, int tamano)
        {
            using var con = _db.CreateConnection();
            using var multi = await con.QueryMultipleAsync("sp_Lista_Entrevista",
                new { p_pagina = pagina, p_tamano = tamano },
                commandType: CommandType.StoredProcedure);
            var items = await multi.ReadAsync<Entrevista>();
            var total = await multi.ReadFirstAsync<int>();
            return (items, total);
        }

        public async Task<Entrevista?> ObtenerPorId(int id)
        {
            using var con = _db.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Entrevista>("sp_Obtener_Entrevista",
                new { p_id_entrevista = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<OferenteResumen>> ObtenerOferentes()
        {
            using var con = _db.CreateConnection();
            return await con.QueryAsync<OferenteResumen>("sp_ObtenerOferentes_Entrevista",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EmpleadoResumen>> ObtenerEmpleados()
        {
            using var con = _db.CreateConnection();
            return await con.QueryAsync<EmpleadoResumen>("sp_ObtenerEmpleados_Entrevista",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Insertar(Entrevista entrevista)
        {
            var dp = new DynamicParameters();
            dp.Add("p_id_oferente", entrevista.IdOferente);
            dp.Add("p_id_empleado", entrevista.IdEmpleado);
            dp.Add("p_fecha_entrevista", entrevista.FechaEntrevista);
            dp.Add("p_id_entrevista", dbType: DbType.Int32, direction: ParameterDirection.Output);
            using var con = _db.CreateConnection();
            await con.ExecuteAsync("sp_Insertar_Entrevista", dp, commandType: CommandType.StoredProcedure);
            return dp.Get<int>("p_id_entrevista");
        }

        public async Task Actualizar(Entrevista entrevista)
        {
            using var con = _db.CreateConnection();
            await con.ExecuteAsync("sp_Actualizar_Entrevista",
                new
                {
                    p_id_entrevista = entrevista.IdEntrevista,
                    p_id_empleado = entrevista.IdEmpleado,
                    p_fecha_entrevista = entrevista.FechaEntrevista
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task MarcarRealizada(int id)
        {
            using var con = _db.CreateConnection();
            await con.ExecuteAsync("sp_MarcarRealizada_Entrevista",
                new { p_id_entrevista = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task Eliminar(int id)
        {
            using var con = _db.CreateConnection();
            await con.ExecuteAsync("sp_Eliminar_Entrevista",
                new { p_id_entrevista = id },
                commandType: CommandType.StoredProcedure);
        }
    }
}
