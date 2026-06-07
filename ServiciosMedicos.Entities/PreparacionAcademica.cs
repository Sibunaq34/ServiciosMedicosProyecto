using System.ComponentModel.DataAnnotations;

namespace Servicios_Medicos.Entities
{
    // Persona C - Kenneth: Entidad para OFE3 Preparacion academica.
    public class PreparacionAcademica : IValidatableObject
    {
        public int IdPreparacion { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El oferente es requerido.")]
        public int IdOferente { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La institucion educativa es requerida.")]
        public int IdInstitucion { get; set; }

        public string CodigoInstitucion { get; set; } = string.Empty;

        public string NombreInstitucion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El titulo obtenido es requerido.")]
        [StringLength(100, ErrorMessage = "El titulo obtenido no puede superar 100 caracteres.")]
        [RegularExpression(@"^[A-Za-z\u00C1\u00C9\u00CD\u00D3\u00DA\u00E1\u00E9\u00ED\u00F3\u00FA\u00D1\u00F1 ]+$", ErrorMessage = "El titulo obtenido solo puede contener letras y espacios.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inicio es requerida.")]
        [DataType(DataType.Date)]
        public DateTime? FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es requerida.")]
        [DataType(DataType.Date)]
        public DateTime? FechaFin { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (FechaInicio.HasValue &&
                FechaFin.HasValue &&
                FechaFin.Value < FechaInicio.Value)
            {
                yield return new ValidationResult(
                    "La fecha de fin debe ser mayor o igual a la fecha de inicio.",
                    new[] { nameof(FechaFin) });
            }
        }
    }
}
