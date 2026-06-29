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
            concurso.Estado = "Vigente";
            Normalizar(concurso);
            Validar(concurso);

            return _repository.CrearAsync(
                concurso,
                idUsuario);
        }

        public Task ActualizarAsync(
            Concurso concurso,
            int idUsuario)
        {
            Normalizar(concurso);
            Validar(concurso);

            return _repository.ActualizarAsync(
                concurso,
                idUsuario);
        }

        public Task CambiarEstadoAsync(
            int idConcurso,
            string estado,
            int idUsuario)
        {
            if (idConcurso <= 0)
                throw new InvalidOperationException(
                    "El concurso es requerido.");

            estado = (estado ?? string.Empty).Trim();

            if (estado != "Vigente" &&
                estado != "Vencido")
            {
                throw new InvalidOperationException(
                    "El estado del concurso debe ser Vigente o Vencido.");
            }

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
                (concurso.Codigo ?? string.Empty).Trim();

            concurso.Nombre =
                (concurso.Nombre ?? string.Empty).Trim();

            concurso.Estado =
                (concurso.Estado ?? string.Empty).Trim();
        }

        private static void Validar(
            Concurso concurso)
        {
            if (string.IsNullOrWhiteSpace(concurso.Codigo))
                throw new InvalidOperationException(
                    "El codigo del concurso es requerido.");

            if (concurso.Codigo.Length > 30)
                throw new InvalidOperationException(
                    "El codigo del concurso no puede superar 30 caracteres.");

            if (string.IsNullOrWhiteSpace(concurso.Nombre))
                throw new InvalidOperationException(
                    "El nombre del concurso es requerido.");

            if (concurso.Nombre.Length > 150)
                throw new InvalidOperationException(
                    "El nombre del concurso no puede superar 150 caracteres.");

            if (!concurso.FechaInicio.HasValue)
                throw new InvalidOperationException(
                    "La fecha de inicio del concurso es requerida.");

            if (!concurso.FechaFin.HasValue)
                throw new InvalidOperationException(
                    "La fecha de fin del concurso es requerida.");

            if (concurso.FechaFin.Value.Date < concurso.FechaInicio.Value.Date)
                throw new InvalidOperationException(
                    "La fecha de fin debe ser mayor o igual a la fecha de inicio.");

            if (string.IsNullOrWhiteSpace(concurso.Estado))
                throw new InvalidOperationException(
                    "El estado del concurso es requerido.");

            if (concurso.Estado != "Vigente" &&
                concurso.Estado != "Vencido")
            {
                throw new InvalidOperationException(
                    "El estado del concurso debe ser Vigente o Vencido.");
            }
        }
    }
}
