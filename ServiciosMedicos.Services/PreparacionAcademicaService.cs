using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;
using System.Text.RegularExpressions;

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
            Validar(preparacion);

            return _repository.CrearAsync(
                preparacion,
                idUsuario);
        }

        public Task ActualizarAsync(
            PreparacionAcademica preparacion,
            int idUsuario)
        {
            Normalizar(preparacion);
            Validar(preparacion);

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
                (preparacion.Titulo ?? string.Empty).Trim();
        }

        private static void Validar(
            PreparacionAcademica preparacion)
        {
            const string letrasRegex =
                @"^[A-Za-zÁÉÍÓÚáéíóúÑñÜü\s]+$";

            if (preparacion.IdOferente <= 0)
                throw new InvalidOperationException(
                    "El oferente es requerido.");

            if (preparacion.IdInstitucion <= 0)
                throw new InvalidOperationException(
                    "Debe seleccionar una institucion educativa.");

            if (string.IsNullOrWhiteSpace(preparacion.Titulo))
                throw new InvalidOperationException(
                    "El titulo obtenido es requerido.");

            if (preparacion.Titulo.Length > 100)
                throw new InvalidOperationException(
                    "El titulo obtenido no puede superar 100 caracteres.");

            if (!Regex.IsMatch(preparacion.Titulo, letrasRegex))
                throw new InvalidOperationException(
                    "El titulo obtenido solo puede contener letras y espacios.");

            if (!preparacion.FechaInicio.HasValue)
                throw new InvalidOperationException(
                    "La fecha de inicio es requerida.");

            if (!preparacion.FechaFin.HasValue)
                throw new InvalidOperationException(
                    "La fecha de fin es requerida.");

            if (preparacion.FechaFin.Value < preparacion.FechaInicio.Value)
                throw new InvalidOperationException(
                    "La fecha de fin debe ser mayor o igual a la fecha de inicio.");
        }
    }
}
