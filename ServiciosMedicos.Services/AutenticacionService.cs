using DocumentFormat.OpenXml.Bibliography;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;
using ZstdSharp;

namespace Servicios_Medicos.Services
{
    public class AutenticacionServices : IUsuario
    {
        private readonly SeguridadRepository _seguridadBD;
        private readonly EncriptadorAESServices _aes;

        public AutenticacionServices(
            SeguridadRepository seguridadBD,
            EncriptadorAESServices aes)
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

        public async Task<SeguridadLog?> Login(string usuario, string password)
        {
            var entidad = await _seguridadBD.ObtenerUsuario(usuario);

            if (entidad == null)
                return null;

            if (entidad.Estado == "Inactivo")
            {
                throw new Exception(
                    "La cuenta está bloqueada.");
            }

            bool valido =
                _aes.CompararPassword(password, entidad.PasswordCifrada);

            if (!valido)
            {
                int intentos = entidad.intentos_fallidos + 1;

                await _seguridadBD.RegistrarIntentoFallido(entidad.IdUsuario, intentos);

                if (intentos >= 3)
                {
                    throw new Exception("La cuenta ha sido bloqueada despues de 3 intentos, contacte al Gerente o departamento de TI");
                }

                throw new Exception(
                    $"La contraseña es incorrecta.");
            }


            await _seguridadBD.RegistrarIntentoFallido(entidad.IdUsuario, 0);

            return entidad;

        }
    }
}