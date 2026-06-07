using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Servicios_Medicos.Entities
{
    // Persona C - Kenneth: Entidad para OFE1 Oferentes.
    public class Oferente : IValidatableObject
    {
        public int IdOferente { get; set; }

        public int IdPersona { get; set; }

        [Required(ErrorMessage = "La identificacion es requerida.")]
        [StringLength(30, ErrorMessage = "La identificacion no puede superar 30 caracteres.")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de identificacion es requerido.")]
        public string TipoIdentificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre completo es requerido.")]
        [StringLength(150, ErrorMessage = "El nombre completo no puede superar 150 caracteres.")]
        [RegularExpression("^[A-Za-z\\u00C1\\u00C9\\u00CD\\u00D3\\u00DA\\u00E1\\u00E9\\u00ED\\u00F3\\u00FA\\u00D1\\u00F1\\u00DC\\u00FC\\s]+$", ErrorMessage = "El nombre completo solo puede contener letras y espacios.")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es requerida.")]
        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }

        public List<string> Correos { get; set; } = new();

        public List<string> Telefonos { get; set; } = new();

        public List<int> ConcursosIds { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (TipoIdentificacion != "CedulaIdentidad" &&
                TipoIdentificacion != "DIMEX" &&
                TipoIdentificacion != "Pasaporte")
            {
                yield return new ValidationResult(
                    "El tipo de identificacion no es valido.",
                    new[] { nameof(TipoIdentificacion) });
            }
            else if (TipoIdentificacion == "CedulaIdentidad" &&
                !Regex.IsMatch(Identificacion, @"^\d{9}$"))
            {
                yield return new ValidationResult(
                    "La cédula debe contener exactamente 9 dígitos numéricos.",
                    new[] { nameof(Identificacion) });
            }
            else if (TipoIdentificacion == "DIMEX" &&
                !Regex.IsMatch(Identificacion, @"^\d{11,12}$"))
            {
                yield return new ValidationResult(
                    "El DIMEX debe contener entre 11 y 12 dígitos numéricos.",
                    new[] { nameof(Identificacion) });
            }
            else if (TipoIdentificacion == "Pasaporte" &&
                !Regex.IsMatch(Identificacion, @"^[A-Za-z0-9]{6,20}$"))
            {
                yield return new ValidationResult(
                    "El pasaporte debe contener entre 6 y 20 caracteres alfanuméricos.",
                    new[] { nameof(Identificacion) });
            }

            if (Correos.Count == 0)
            {
                yield return new ValidationResult(
                    "Debe indicar al menos un correo electronico.",
                    new[] { nameof(Correos) });
            }

            if (Telefonos.Count == 0)
            {
                yield return new ValidationResult(
                    "Debe indicar al menos un telefono.",
                    new[] { nameof(Telefonos) });
            }

            foreach (var telefono in Telefonos)
            {
                if (!Regex.IsMatch(telefono, @"^\d{8}$"))
                {
                    yield return new ValidationResult(
                        "El teléfono debe contener exactamente 8 dígitos numéricos.",
                        new[] { nameof(Telefonos) });
                }
            }

            if (ConcursosIds.Count == 0)
            {
                yield return new ValidationResult(
                    "Debe seleccionar al menos un concurso.",
                    new[] { nameof(ConcursosIds) });
            }
        }
    }
}
