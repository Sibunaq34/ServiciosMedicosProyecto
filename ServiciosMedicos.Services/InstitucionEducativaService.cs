using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;
using System.Text.RegularExpressions;

namespace Servicios_Medicos.Services
{
    // Persona C - Kenneth: Servicio de aplicacion para GEN5.
    public class InstitucionEducativaService : IInstitucionEducativaService
    {
        private readonly InstitucionEducativaRepository _repository;

        public InstitucionEducativaService(
            InstitucionEducativaRepository repository)
        {
            _repository = repository;
        }

        public Task<IReadOnlyList<InstitucionEducativa>> ListarAsync(
            int pagina,
            int tamanoPagina,
            int idUsuario)
        {
            return _repository.ListarAsync(
                pagina,
                tamanoPagina,
                idUsuario);
        }

        public Task<InstitucionEducativa?> ObtenerAsync(
            int idInstitucion,
            int idUsuario)
        {
            return _repository.ObtenerAsync(
                idInstitucion,
                idUsuario);
        }

        public Task<int> CrearAsync(
            InstitucionEducativa institucion,
            int idUsuario)
        {
            Normalizar(institucion);
            Validar(institucion);

            return _repository.CrearAsync(
                institucion,
                idUsuario);
        }

        public Task ActualizarAsync(
            InstitucionEducativa institucion,
            int idUsuario)
        {
            Normalizar(institucion);
            Validar(institucion);

            return _repository.ActualizarAsync(
                institucion,
                idUsuario);
        }

        public Task EliminarAsync(
            int idInstitucion,
            int idUsuario)
        {
            return _repository.EliminarAsync(
                idInstitucion,
                idUsuario);
        }

        private static void Normalizar(
            InstitucionEducativa institucion)
        {
            institucion.Codigo =
                (institucion.Codigo ?? string.Empty).Trim();

            institucion.Nombre =
                (institucion.Nombre ?? string.Empty).Trim();
        }

        private static void Validar(
            InstitucionEducativa institucion)
        {
            const string letrasRegex =
                @"^[A-Za-zÁÉÍÓÚáéíóúÑñÜü\s]+$";

            if (string.IsNullOrWhiteSpace(institucion.Codigo))
                throw new InvalidOperationException(
                    "El codigo de la institucion es requerido.");

            if (institucion.Codigo.Length > 30)
                throw new InvalidOperationException(
                    "El codigo de la institucion no puede superar 30 caracteres.");

            if (string.IsNullOrWhiteSpace(institucion.Nombre))
                throw new InvalidOperationException(
                    "El nombre de la institucion es requerido.");

            if (institucion.Nombre.Length > 150)
                throw new InvalidOperationException(
                    "El nombre de la institucion no puede superar 150 caracteres.");

            if (!Regex.IsMatch(institucion.Nombre, letrasRegex))
                throw new InvalidOperationException(
                    "El nombre solo puede contener letras y espacios.");
        }
    }
}
