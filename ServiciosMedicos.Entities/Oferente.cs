using System.ComponentModel.DataAnnotations;

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

            if (ConcursosIds.Count == 0)
            {
                yield return new ValidationResult(
                    "Debe seleccionar al menos un concurso.",
                    new[] { nameof(ConcursosIds) });
            }
        }
    }
}
