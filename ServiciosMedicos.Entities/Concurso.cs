using System.ComponentModel.DataAnnotations;

namespace Servicios_Medicos.Entities
{
    // Persona C - Kenneth: Entidad para OFE2 Concursos.
    public class Concurso : IValidatableObject
    {
        public int IdConcurso { get; set; }

        [Required(ErrorMessage = "El codigo del concurso es requerido.")]
        [StringLength(30, ErrorMessage = "El codigo del concurso no puede superar 30 caracteres.")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del concurso es requerido.")]
        [StringLength(150, ErrorMessage = "El nombre del concurso no puede superar 150 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inicio del concurso es requerida.")]
        [DataType(DataType.Date)]
        public DateTime? FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin del concurso es requerida.")]
        [DataType(DataType.Date)]
        public DateTime? FechaFin { get; set; }

        [Required(ErrorMessage = "El estado del concurso es requerido.")]
        public string Estado { get; set; } = "Vigente";

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (FechaInicio.HasValue &&
                FechaFin.HasValue &&
                FechaFin.Value.Date < FechaInicio.Value.Date)
            {
                yield return new ValidationResult(
                    "La fecha de fin debe ser mayor o igual a la fecha de inicio.",
                    new[] { nameof(FechaFin) });
            }

            if (Estado != "Vigente" &&
                Estado != "Vencido")
            {
                yield return new ValidationResult(
                    "El estado del concurso debe ser Vigente o Vencido.",
                    new[] { nameof(Estado) });
            }
        }
    }
}
