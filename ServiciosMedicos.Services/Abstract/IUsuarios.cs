using Servicios_Medicos.Entities;

namespace Servicios_Medicos.Services.Abstract
{
    public interface IUsuario
    {
        Task<bool> RegistrarUsuario(
            SeguridadLog usuario);

        Task<SeguridadLog?> Login(
            string usuario,
            string password);
    }
}