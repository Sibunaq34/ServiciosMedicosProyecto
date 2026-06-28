using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;
using System.Text.RegularExpressions;

namespace Servicios_Medicos.Services
{
    public class UsuariosAdminServices : IUsuariosAdmin
    {
        private readonly UsuariosAdminRepository _usuariosBD;
        private readonly EncriptadorAESServices _encriptadorAES;

        public UsuariosAdminServices(UsuariosAdminRepository usuariosBD, EncriptadorAESServices encriptadorAES)
        {
            _usuariosBD = usuariosBD;
            _encriptadorAES = encriptadorAES;
        }

        public Task<IEnumerable<UsuarioAdmin>> Listar()
        {
            return _usuariosBD.Listar();
        }

        public Task<UsuarioAdmin?> ObtenerPorId(int idUsuario)
        {
            return _usuariosBD.ObtenerPorId(idUsuario);
        }

        public async Task Crear(UsuarioAdmin usuario)
        {
            Validar(usuario, true);

            usuario.Contrasena =
                _encriptadorAES.Encriptar(usuario.Contrasena);

            await _usuariosBD.Crear(usuario);
        }

        public async Task Actualizar(UsuarioAdmin usuario)
        {
            Validar(usuario, false);

            await _usuariosBD.Actualizar(usuario);
        }

        public Task CambiarEstado(int idUsuario, bool activo)
        {
            return _usuariosBD.CambiarEstado(idUsuario, activo);
        }

        public Task Eliminar(int idUsuario)
        {
            return _usuariosBD.Eliminar(idUsuario);
        }

        private static void Validar(
            UsuarioAdmin usuario,
            bool validarContrasena)
        {
            if (string.IsNullOrWhiteSpace(usuario.UsuarioNombre))
                throw new Exception("El nombre de usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(usuario.NombreCompleto))
                throw new Exception("El nombre completo es obligatorio.");

            if (string.IsNullOrWhiteSpace(usuario.Correo))
                throw new Exception("El correo es obligatorio.");

            if (usuario.IdRol == null || usuario.IdRol <= 0)
                throw new Exception("Debe seleccionar un rol.");

            if (string.IsNullOrWhiteSpace(usuario.Estado))
                throw new Exception("Debe indicar el estado.");

            if (validarContrasena)
            {
                if (string.IsNullOrWhiteSpace(usuario.Contrasena))
                    throw new Exception("La contraseña es obligatoria.");

                if (usuario.Contrasena.Length < 8)
                    throw new Exception("La contraseña debe tener mínimo 8 caracteres.");

                if (!Regex.IsMatch(usuario.Contrasena, @"[A-Z]"))
                    throw new Exception("La contraseña debe tener una mayúscula.");

                if (!Regex.IsMatch(usuario.Contrasena, @"[a-z]"))
                    throw new Exception("La contraseña debe tener una minúscula.");

                if (!Regex.IsMatch(usuario.Contrasena, @"[0-9]"))
                    throw new Exception("La contraseña debe tener un número.");

                if (!Regex.IsMatch(usuario.Contrasena, @"[\W_]"))
                    throw new Exception("La contraseña debe tener un carácter especial.");
            }
        }
    }
}
