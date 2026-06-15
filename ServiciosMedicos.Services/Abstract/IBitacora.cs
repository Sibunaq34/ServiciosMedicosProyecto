using ServiciosMedicos.Entities;

namespace Servicios_Medicos.Services.Abstract
{
    public interface IBitacora
    {
        Task<IEnumerable<ServiciosMedicos.Entities.Bitacora>> ConsultarBitacoras(
            string? usuario,
            string? descripcion);
    }
}