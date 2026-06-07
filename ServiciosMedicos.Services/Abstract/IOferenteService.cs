using Servicios_Medicos.Entities;

namespace Servicios_Medicos.Services.Abstract
{
    // Persona C - Kenneth: Contrato de servicio para OFE1.
    public interface IOferenteService
    {
        Task<IReadOnlyList<OferenteListado>> ListarAsync(
            int pagina,
            int tamanoPagina,
            int idUsuario);

        Task<OferenteDetalle?> ObtenerAsync(
            int idOferente,
            int idUsuario);

        Task<int> CrearAsync(
            Oferente oferente,
            int idUsuario);

        Task ActualizarAsync(
            Oferente oferente,
            int idUsuario);

        Task EliminarAsync(
            int idOferente,
            int idUsuario);
    }
}
