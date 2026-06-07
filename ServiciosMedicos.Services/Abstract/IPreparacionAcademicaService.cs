using Servicios_Medicos.Entities;

namespace Servicios_Medicos.Services.Abstract
{
    // Persona C - Kenneth: Contrato de servicio para OFE3.
    public interface IPreparacionAcademicaService
    {
        Task<IReadOnlyList<PreparacionAcademica>> ListarPorOferenteAsync(
            int idOferente,
            int pagina,
            int tamanoPagina,
            int idUsuario);

        Task<PreparacionAcademica?> ObtenerAsync(
            int idPreparacion,
            int idUsuario);

        Task<int> CrearAsync(
            PreparacionAcademica preparacion,
            int idUsuario);

        Task ActualizarAsync(
            PreparacionAcademica preparacion,
            int idUsuario);

        Task EliminarAsync(
            int idPreparacion,
            int idUsuario);
    }
}
