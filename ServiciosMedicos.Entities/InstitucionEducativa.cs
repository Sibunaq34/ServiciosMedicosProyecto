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
        [RegularExpression("^[A-Za-z\\u00C1\\u00C9\\u00CD\\u00D3\\u00DA\\u00E1\\u00E9\\u00ED\\u00F3\\u00FA\\u00D1\\u00F1\\u00DC\\u00FC\\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
