using ServiciosMedicos.Entities;

namespace Servicios_Medicos.Services.Abstract
{
    public interface IBitacora
    {
        Task<IReadOnlyList<Bitacora>> ConsultarBitacoras(string? usuario,string? descripcion,int pagina, int tamanoPagina);
    }
}