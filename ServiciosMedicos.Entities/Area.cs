namespace Servicios_Medicos.Entities
{
    public class Area
    {
        public int IdArea { get; set; }
        public string CodigoArea { get; set; } = string.Empty;
        public string NombreArea { get; set; } = string.Empty;
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
    }
}
