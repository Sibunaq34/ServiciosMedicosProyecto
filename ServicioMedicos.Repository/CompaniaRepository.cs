using Dapper;
using Servicios_Medicos.Entities;

namespace Servicios_Medicos.Repository
{
    public class CompaniaRepository
    {
        private readonly IDbConnectionFactory _db;

        public CompaniaRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<Compania> Items, int Total)> ObtenerPaginado(int pagina, int tamano = 10)
        {
            using var con = _db.CreateConnection();
            var offset = (pagina - 1) * tamano;

            var items = await con.QueryAsync<Compania>(
                @"SELECT id_compania AS IdCompania,
                         codigo_compania AS CodigoCompania,
                         nombre AS Nombre
                  FROM companias
                  ORDER BY codigo_compania
                  LIMIT @Tamano OFFSET @Offset",
                new { Tamano = tamano, Offset = offset });

            var total = await con.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM companias");

            return (items, total);
        }

        public async Task<Compania?> ObtenerPorId(int id)
        {
            using var con = _db.CreateConnection();
            return await con.QuerySingleOrDefaultAsync<Compania>(
                @"SELECT id_compania AS IdCompania,
                         codigo_compania AS CodigoCompania,
                         nombre AS Nombre
                  FROM companias
                  WHERE id_compania = @Id",
                new { Id = id });
        }

        public async Task<bool> CodigoExiste(string codigo, int? excluirId = null)
        {
            using var con = _db.CreateConnection();
            string sql = excluirId.HasValue
                ? "SELECT COUNT(1) FROM companias WHERE codigo_compania = @Codigo AND id_compania <> @ExcluirId"
                : "SELECT COUNT(1) FROM companias WHERE codigo_compania = @Codigo";

            var count = await con.ExecuteScalarAsync<int>(sql, new { Codigo = codigo, ExcluirId = excluirId });
            return count > 0;
        }

        public async Task Insertar(Compania compania)
        {
            using var con = _db.CreateConnection();
            await con.ExecuteAsync(
                "INSERT INTO companias (codigo_compania, nombre) VALUES (@CodigoCompania, @Nombre)",
                compania);
        }

        public async Task Actualizar(Compania compania)
        {
            using var con = _db.CreateConnection();
            await con.ExecuteAsync(
                "UPDATE companias SET codigo_compania = @CodigoCompania, nombre = @Nombre WHERE id_compania = @IdCompania",
                compania);
        }

        public async Task<bool> TieneRelaciones(int id)
        {
            // Ninguna tabla actual referencia companias
            // Mantener para cuando se agreguen FKs en el futuro
            return await Task.FromResult(false);
        }

        public async Task Eliminar(int id)
        {
            using var con = _db.CreateConnection();
            await con.ExecuteAsync(
                "DELETE FROM companias WHERE id_compania = @Id",
                new { Id = id });
        }
    }
}
