using System.ComponentModel.DataAnnotations;

namespace Servicios_Medicos.Entities
{
    public class Parametros
    {
        public int IdParametro { get; set; }

        [Required(ErrorMessage = "El código es requerido")]
        public string CodigoParametro { get; set; } = string.Empty;

        [Required(ErrorMessage = "El valor es requerido")]
        [MaxLength(500,
            ErrorMessage = "Máximo 500 caracteres")]
        public string Valor { get; set; } = string.Empty;
    }
}