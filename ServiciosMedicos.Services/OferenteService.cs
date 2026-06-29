using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Servicios_Medicos.Services
{
    // Persona C - Kenneth: Servicio de aplicacion para OFE1.
    public class OferenteService : IOferenteService
    {
        private readonly OferenteRepository _repository;

        public OferenteService(
            OferenteRepository repository)
        {
            _repository = repository;
        }

        public Task<IReadOnlyList<OferenteListado>> ListarAsync(
            int pagina,
            int tamanoPagina,
            int idUsuario)
        {
            return _repository.ListarAsync(
                pagina,
                tamanoPagina,
                idUsuario);
        }

        public Task<OferenteDetalle?> ObtenerAsync(
            int idOferente,
            int idUsuario)
        {
            return _repository.ObtenerAsync(
                idOferente,
                idUsuario);
        }

        public Task<int> CrearAsync(
            Oferente oferente,
            int idUsuario)
        {
            Normalizar(oferente);
            Validar(oferente);

            return _repository.CrearAsync(
                oferente,
                idUsuario);
        }

        public Task ActualizarAsync(
            Oferente oferente,
            int idUsuario)
        {
            Normalizar(oferente);
            Validar(oferente);

            return _repository.ActualizarAsync(
                oferente,
                idUsuario);
        }

        public Task EliminarAsync(
            int idOferente,
            int idUsuario)
        {
            return _repository.EliminarAsync(
                idOferente,
                idUsuario);
        }

        private static void Normalizar(
            Oferente oferente)
        {
            oferente.TipoIdentificacion =
                (oferente.TipoIdentificacion ?? string.Empty).Trim();

            oferente.Identificacion = NormalizarIdentificacion(
                oferente.Identificacion,
                oferente.TipoIdentificacion);

            oferente.NombreCompleto =
                (oferente.NombreCompleto ?? string.Empty).Trim();

            oferente.Correos =
                (oferente.Correos ?? new List<string>())
                    .Select(correo => correo.Trim())
                    .Where(correo => !string.IsNullOrWhiteSpace(correo))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

            oferente.Telefonos =
                (oferente.Telefonos ?? new List<string>())
                    .Select(telefono => telefono.Trim())
                    .Where(telefono => !string.IsNullOrWhiteSpace(telefono))
                    .Distinct()
                    .ToList();

            oferente.ConcursosIds =
                (oferente.ConcursosIds ?? new List<int>())
                    .Where(id => id > 0)
                    .Distinct()
                    .ToList();
        }

        private static string NormalizarIdentificacion(
            string? identificacion,
            string tipoIdentificacion)
        {
            var valor = (identificacion ?? string.Empty).Trim();

            if (tipoIdentificacion == "CedulaIdentidad" ||
                tipoIdentificacion == "DIMEX")
            {
                valor = Regex.Replace(valor, @"[\s-]", string.Empty);
            }

            return valor;
        }

        private static void Validar(
            Oferente oferente)
        {
            const string letrasRegex =
                @"^[A-Za-zÁÉÍÓÚáéíóúÑñÜü\s]+$";

            if (string.IsNullOrWhiteSpace(oferente.Identificacion))
                throw new InvalidOperationException(
                    "La identificacion es requerida.");

            if (oferente.Identificacion.Length > 30)
                throw new InvalidOperationException(
                    "La identificacion no puede superar 30 caracteres.");

            if (string.IsNullOrWhiteSpace(oferente.TipoIdentificacion))
                throw new InvalidOperationException(
                    "El tipo de identificacion es requerido.");

            if (oferente.TipoIdentificacion != "CedulaIdentidad" &&
                oferente.TipoIdentificacion != "DIMEX" &&
                oferente.TipoIdentificacion != "Pasaporte")
            {
                throw new InvalidOperationException(
                    "El tipo de identificacion no es valido.");
            }

            if (oferente.TipoIdentificacion == "CedulaIdentidad" &&
                !Regex.IsMatch(oferente.Identificacion, @"^\d{9}$"))
            {
                throw new InvalidOperationException(
                    "La cedula debe contener exactamente 9 digitos numericos.");
            }

            if (oferente.TipoIdentificacion == "DIMEX" &&
                !Regex.IsMatch(oferente.Identificacion, @"^\d{11,12}$"))
            {
                throw new InvalidOperationException(
                    "El DIMEX debe contener entre 11 y 12 digitos numericos.");
            }

            if (oferente.TipoIdentificacion == "Pasaporte" &&
                !Regex.IsMatch(oferente.Identificacion, @"^[A-Za-z0-9]{6,20}$"))
            {
                throw new InvalidOperationException(
                    "El pasaporte debe contener entre 6 y 20 caracteres alfanumericos.");
            }

            if (string.IsNullOrWhiteSpace(oferente.NombreCompleto))
                throw new InvalidOperationException(
                    "El nombre completo es requerido.");

            if (oferente.NombreCompleto.Length > 150)
                throw new InvalidOperationException(
                    "El nombre completo no puede superar 150 caracteres.");

            if (!Regex.IsMatch(oferente.NombreCompleto, letrasRegex))
                throw new InvalidOperationException(
                    "El nombre completo solo puede contener letras y espacios.");

            if (!oferente.FechaNacimiento.HasValue)
                throw new InvalidOperationException(
                    "La fecha de nacimiento es requerida.");

            if (oferente.Correos.Count == 0)
                throw new InvalidOperationException(
                    "Debe indicar al menos un correo electronico.");

            var emailValidator = new EmailAddressAttribute();

            foreach (var correo in oferente.Correos)
            {
                if (!emailValidator.IsValid(correo))
                    throw new InvalidOperationException(
                        $"El correo '{correo}' no tiene un formato valido.");
            }

            if (oferente.Telefonos.Count == 0)
                throw new InvalidOperationException(
                    "Debe indicar al menos un telefono.");

            foreach (var telefono in oferente.Telefonos)
            {
                if (!Regex.IsMatch(telefono, @"^\d{8}$"))
                    throw new InvalidOperationException(
                        "El telefono debe contener exactamente 8 digitos numericos.");
            }

            if (oferente.ConcursosIds.Count == 0)
                throw new InvalidOperationException(
                    "Debe seleccionar al menos un concurso.");
        }
    }
}
