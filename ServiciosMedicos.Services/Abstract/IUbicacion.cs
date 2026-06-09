using Microsoft.AspNetCore.Http;

namespace Servicios_Medicos.Services.Abstract
{
    public interface IUbicacion
    {
        Task CargarUbicaciones(IFormFile archivo, int idusuario);
    }
}