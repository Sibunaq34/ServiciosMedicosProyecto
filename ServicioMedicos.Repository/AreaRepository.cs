using Dapper;
using Servicios_Medicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository
{
    public class AreaRepository
    {
        private readonly IDbConnectionFactory _db;

        public AreaRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<Area> Items, int Total)> ObtenerPaginado(int pagina, int tamano)
        {
            using var con = _db.CreateConnection();
            using var multi = await con.QueryMultipleAsync("sp_Lista_Area",
                new { p_pagina = pagina, p_tamano = tamano },
                commandType: CommandType.StoredProcedure);
            var items = await multi.ReadAsync<Area>();
            var total = await multi.ReadFirstAsync<int>();
            return (items, total);
        }

        public async Task<Area?> ObtenerPorId(int id)
        {
            using var con = _db.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Area>("sp_Obtener_Area",
                new { p_id_area = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EmpleadoResumen>> ObtenerEmpleados()
        {
            using var con = _db.CreateConnection();
            return await con.QueryAsync<EmpleadoResumen>("sp_ObtenerEmpleados_Area",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> CodigoExiste(string codigo, int? idArea)
        {
            var dp = new DynamicParameters();
            dp.Add("p_codigo_area", codigo);
            dp.Add("p_id_area", idArea);
            dp.Add("p_existe", dbType: DbType.Int32, direction: ParameterDirection.Output);
            using var con = _db.CreateConnection();
            await con.ExecuteAsync("sp_CodigoExiste_Area", dp, commandType: CommandType.StoredProcedure);
            return dp.Get<int>("p_existe") > 0;
        }

        public async Task<int> Insertar(Area area)
        {
            var dp = new DynamicParameters();
            dp.Add("p_codigo_area", area.CodigoArea);
            dp.Add("p_nombre_area", area.NombreArea);
            dp.Add("p_id_empleado", area.IdEmpleado);
            dp.Add("p_id_area", dbType: DbType.Int32, direction: ParameterDirection.Output);
            using var con = _db.CreateConnection();
            await con.ExecuteAsync("sp_Insertar_Area", dp, commandType: CommandType.StoredProcedure);
            return dp.Get<int>("p_id_area");
        }

        public async Task Actualizar(Area area)
        {
            using var con = _db.CreateConnection();
            await con.ExecuteAsync("sp_Actualizar_Area",
                new
                {
                    p_id_area = area.IdArea,
                    p_codigo_area = area.CodigoArea,
                    p_nombre_area = area.NombreArea,
                    p_id_empleado = area.IdEmpleado
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task Eliminar(int id)
        {
            using var con = _db.CreateConnection();
            await con.ExecuteAsync("sp_Eliminar_Area",
                new { p_id_area = id },
                commandType: CommandType.StoredProcedure);
        }
    }
}
