using Dapper;
using Servicios_Medicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository
{
    public class UsuariosAdminRepository
    {
        private readonly IDbConnectionFactory _db;

        public UsuariosAdminRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<IEnumerable<UsuarioAdmin>> Listar()
        {
            using var connection = _db.CreateConnection();

            return await connection.QueryAsync<UsuarioAdmin>(
                "sp_Usuarios_Listar",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<UsuarioAdmin?> ObtenerPorId(int idUsuario)
        {
            using var connection = _db.CreateConnection();

            var sql = @"
        CALL sp_Usuarios_ObtenerPorId(@p_idUsuario);
    ";

            var usuario = await connection.QueryFirstOrDefaultAsync(sql, new
            {
                p_idUsuario = idUsuario
            });

            if (usuario == null)
                return null;

            return new UsuarioAdmin
            {
                IdUsuario = usuario.id_usuario,
                UsuarioNombre = usuario.usuario,
                NombreCompleto = usuario.nombre_completo,
                Correo = usuario.correo,
                Estado = usuario.estado,
                Activo = usuario.estado == "Activo",
                IdRol = usuario.id_rol,
                NombrePermiso = usuario.nombre_permiso
            };
        }

        public async Task Crear(UsuarioAdmin usuario)
        {
            using var connection = _db.CreateConnection();

            await connection.ExecuteAsync(
                "sp_Usuarios_Crear",
                new
                {
                    p_usuario = usuario.UsuarioNombre,
                    p_nombre_completo = usuario.NombreCompleto,
                    p_correo = usuario.Correo,
                    p_contrasena = usuario.Contrasena,
                    p_estado = usuario.Estado,
                    p_id_rol = usuario.IdRol
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task Actualizar(UsuarioAdmin usuario)
        {
            using var connection = _db.CreateConnection();

            await connection.ExecuteAsync(
                "sp_Usuarios_Actualizar",
                new
                {
                    p_idUsuario = usuario.IdUsuario,
                    p_usuario = usuario.UsuarioNombre,
                    p_nombreCompleto = usuario.NombreCompleto,
                    p_correo = usuario.Correo,
                    p_idRol = usuario.IdRol
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task CambiarEstado(int idUsuario, bool activo)
        {
            using var connection = _db.CreateConnection();

            await connection.ExecuteAsync(
                "sp_Usuarios_CambiarEstado",
                new
                {
                    p_idUsuario = idUsuario,
                    p_estado = activo ? "Activo" : "Inactivo"
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task Eliminar(int idUsuario)
        {
            using var connection = _db.CreateConnection();

            await connection.ExecuteAsync(
                "sp_Usuarios_Eliminar",
                new { p_idUsuario = idUsuario },
                commandType: CommandType.StoredProcedure);
        }
    }
}