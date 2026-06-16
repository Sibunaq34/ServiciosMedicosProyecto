using ServiciosMedicos.Entities;

namespace Servicios_Medicos.Services.Abstract
{
    public interface IBitacora
    {
        Task<IEnumerable<BitacoraEntidad>> ConsultarBitacoras(
            string? usuario,
            string? descripcion);
    }
}