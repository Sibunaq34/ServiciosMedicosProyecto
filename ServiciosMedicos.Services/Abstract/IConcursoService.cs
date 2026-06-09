using Servicios_Medicos.Entities;

namespace Servicios_Medicos.Services.Abstract
{
    // Persona C - Kenneth: Contrato de servicio para OFE2.
    public interface IConcursoService
    {
        Task<IReadOnlyList<Concurso>> ListarAsync(
            int pagina,
            int tamanoPagina,
            int idUsuario);

        Task<Concurso?> ObtenerAsync(
            int idConcurso,
            int idUsuario);

        Task<int> CrearAsync(
            Concurso concurso,
            int idUsuario);

        Task ActualizarAsync(
            Concurso concurso,
            int idUsuario);

        Task CambiarEstadoAsync(
            int idConcurso,
            string estado,
            int idUsuario);

        Task EliminarAsync(
            int idConcurso,
            int idUsuario);
    }
}
