using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;

namespace Servicios_Medicos.Services
{
    // Persona C - Kenneth: Servicio de aplicacion para OFE3.
    public class PreparacionAcademicaService : IPreparacionAcademicaService
    {
        private readonly PreparacionAcademicaRepository _repository;

        public PreparacionAcademicaService(
            PreparacionAcademicaRepository repository)
        {
            _repository = repository;
        }

        public Task<IReadOnlyList<PreparacionAcademica>> ListarPorOferenteAsync(
            int idOferente,
            int pagina,
            int tamanoPagina,
            int idUsuario)
        {
            return _repository.ListarPorOferenteAsync(
                idOferente,
                pagina,
                tamanoPagina,
                idUsuario);
        }

        public Task<PreparacionAcademica?> ObtenerAsync(
            int idPreparacion,
            int idUsuario)
        {
            return _repository.ObtenerAsync(
                idPreparacion,
                idUsuario);
        }

        public Task<int> CrearAsync(
            PreparacionAcademica preparacion,
            int idUsuario)
        {
            Normalizar(preparacion);

            return _repository.CrearAsync(
                preparacion,
                idUsuario);
        }

        public Task ActualizarAsync(
            PreparacionAcademica preparacion,
            int idUsuario)
        {
            Normalizar(preparacion);

            return _repository.ActualizarAsync(
                preparacion,
                idUsuario);
        }

        public Task EliminarAsync(
            int idPreparacion,
            int idUsuario)
        {
            return _repository.EliminarAsync(
                idPreparacion,
                idUsuario);
        }

        private static void Normalizar(
            PreparacionAcademica preparacion)
        {
            preparacion.Titulo =
                preparacion.Titulo.Trim();
        }
    }
}
