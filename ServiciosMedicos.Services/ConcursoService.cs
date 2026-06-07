using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;

namespace Servicios_Medicos.Services
{
    // Persona C - Kenneth: Servicio de aplicacion para OFE2.
    public class ConcursoService : IConcursoService
    {
        private readonly ConcursoRepository _repository;

        public ConcursoService(
            ConcursoRepository repository)
        {
            _repository = repository;
        }

        public Task<IReadOnlyList<Concurso>> ListarAsync(
            int pagina,
            int tamanoPagina,
            int idUsuario)
        {
            return _repository.ListarAsync(
                pagina,
                tamanoPagina,
                idUsuario);
        }

        public Task<Concurso?> ObtenerAsync(
            int idConcurso,
            int idUsuario)
        {
            return _repository.ObtenerAsync(
                idConcurso,
                idUsuario);
        }

        public Task<int> CrearAsync(
            Concurso concurso,
            int idUsuario)
        {
            Normalizar(concurso);
            concurso.Estado = "Vigente";

            return _repository.CrearAsync(
                concurso,
                idUsuario);
        }

        public Task ActualizarAsync(
            Concurso concurso,
            int idUsuario)
        {
            Normalizar(concurso);

            return _repository.ActualizarAsync(
                concurso,
                idUsuario);
        }

        public Task CambiarEstadoAsync(
            int idConcurso,
            string estado,
            int idUsuario)
        {
            return _repository.CambiarEstadoAsync(
                idConcurso,
                estado,
                idUsuario);
        }

        public Task EliminarAsync(
            int idConcurso,
            int idUsuario)
        {
            return _repository.EliminarAsync(
                idConcurso,
                idUsuario);
        }

        private static void Normalizar(
            Concurso concurso)
        {
            concurso.Codigo =
                concurso.Codigo.Trim();

            concurso.Nombre =
                concurso.Nombre.Trim();

            concurso.Estado =
                concurso.Estado.Trim();
        }
    }
}
