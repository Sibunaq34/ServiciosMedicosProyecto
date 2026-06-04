using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;

namespace Servicios_Medicos.Services
{
    public class Autenticacion : IUsuario
    {
        private readonly SeguridadBD _seguridadBD;
        private readonly EncriptadorAES _aes;

        public Autenticacion(
            SeguridadBD seguridadBD,
            EncriptadorAES aes)
        {
            _seguridadBD = seguridadBD;
            _aes = aes;
        }

        public async Task<bool> RegistrarUsuario(
            SeguridadLog usuario)
        {
            usuario.PasswordCifrada =
                _aes.Encriptar(usuario.Password);

            return await _seguridadBD
                .RegistrarUsuario(usuario);
        }

        public async Task<SeguridadLog?> Login(
            string usuario,
            string password)
        {
            var entidad =
                await _seguridadBD
                    .ObtenerUsuario(usuario);

            if (entidad == null)
                return null;

            bool valido =
                _aes.CompararPassword(
                    password,
                    entidad.PasswordCifrada);

            return valido ? entidad : null;
        }
    }
}