using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;
using ZstdSharp;

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

            if (valido == false)
            {
                var intentos = await _seguridadBD.RegistrarIntentoFallido(entidad.usuario);
                if (intentos = 3) {
                    throw new Exception("La cuenta esta bloqueada");
                }

                throw new Exception("La contrasena es incorrecta");
            
            }


            return valido ? entidad : null;
        }
    }
}