namespace Servicios_Medicos.Entities
{
    public class ParametroEntidad
    {
        public int IdParametro { get; set; }

        public string CodigoParametro { get; set; } = string.Empty;

        public string Valor { get; set; } = string.Empty;
    }
}