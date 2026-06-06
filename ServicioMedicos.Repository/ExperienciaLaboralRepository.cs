using Dapper;
using Servicios_Medicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository
{
    public class ExperienciaLaboralRepository
    {
        private readonly IDbConnectionFactory _db;

        public ExperienciaLaboralRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<ExperienciaLaboral> Items, int Total)> ObtenerPaginado(int idOferente, int pagina, int tamano)
        {
            using var con = _db.CreateConnection();
            using var multi = await con.QueryMultipleAsync("sp_Lista_Experiencia_Laboral",
                new { p_id_oferente = idOferente, p_pagina = pagina, p_tamano = tamano },
                commandType: CommandType.StoredProcedure);
            var items = await multi.ReadAsync<ExperienciaLaboral>();
            var total = await multi.ReadFirstAsync<int>();
            return (items, total);
        }

        public async Task<ExperienciaLaboral?> ObtenerPorId(int id)
        {
            using var con = _db.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<ExperienciaLaboral>("sp_ObtenerExperiencia_Laboral",
                new { p_id_experiencia = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<string?> ObtenerNombreOferente(int idOferente)
        {
            using var con = _db.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<string>("sp_ObtenerNombreOferente",
                new { p_id_oferente = idOferente },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Insertar(ExperienciaLaboral exp)
        {
            var dp = new DynamicParameters();
            dp.Add("p_id_oferente", exp.IdOferente);
            dp.Add("p_nombre_empresa", exp.NombreEmpresa);
            dp.Add("p_puesto_desempenado", exp.PuestoDesempenado);
            dp.Add("p_fecha_inicio", exp.FechaInicio);
            dp.Add("p_fecha_fin", exp.FechaFin);
            dp.Add("p_id_experiencia", dbType: DbType.Int32, direction: ParameterDirection.Output);
            using var con = _db.CreateConnection();
            await con.ExecuteAsync("sp_Insertar_Experiencia_Laboral", dp, commandType: CommandType.StoredProcedure);
            return dp.Get<int>("p_id_experiencia");
        }

        public async Task Actualizar(ExperienciaLaboral exp)
        {
            using var con = _db.CreateConnection();
            await con.ExecuteAsync("sp_Actualizar_Experiencia_Laboal",
                new
                {
                    p_id_experiencia = exp.IdExperiencia,
                    p_nombre_empresa = exp.NombreEmpresa,
                    p_puesto_desempenado = exp.PuestoDesempenado,
                    p_fecha_inicio = exp.FechaInicio,
                    p_fecha_fin = exp.FechaFin
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task Eliminar(int id)
        {
            using var con = _db.CreateConnection();
            await con.ExecuteAsync("sp_Eliminar_Experiencia_Laboral",
                new { p_id_experiencia = id },
                commandType: CommandType.StoredProcedure);
        }
    }
}
