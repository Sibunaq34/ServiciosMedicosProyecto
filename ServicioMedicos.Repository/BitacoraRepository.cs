using Dapper;
using System.Text.Json;

namespace Servicios_Medicos.Repository
{
    public class BitacoraRepository
    {
        private readonly IDbConnectionFactory _db;

        public BitacoraRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task Registrar(int idUsuario, string accion, object descripcion)
        {
            //Se convierte a JSOn
            var json = descripcion is string s
                ? JsonSerializer.Serialize(new { mensaje = s })
                : JsonSerializer.Serialize(descripcion);

            using var con = _db.CreateConnection();
            await con.ExecuteAsync(
                "INSERT INTO bitacoras (id_usuario, accion, descripcionAccion) VALUES (@IdUsuario, @Accion, @Json)",
                new { IdUsuario = idUsuario, Accion = accion, Json = json });
        }
    }
}
