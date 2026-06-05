using System.ComponentModel.DataAnnotations;

namespace Servicios_Medicos.Entities
{
    // Persona C - Kenneth: Entidad para GEN5 Instituciones educativas.
    public class InstitucionEducativa
    {
        public int IdInstitucion { get; set; }

        [Required(ErrorMessage = "El codigo de la institucion es requerido.")]
        [StringLength(30, ErrorMessage = "El codigo de la institucion no puede superar 30 caracteres.")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de la institucion es requerido.")]
        [StringLength(150, ErrorMessage = "El nombre de la institucion no puede superar 150 caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ ]+$", ErrorMessage = "El nombre de la institucion solo puede contener letras y espacios.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
