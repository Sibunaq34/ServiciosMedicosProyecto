namespace Servicios_Medicos.Entities
{
    public class Entrevista
    {
        public int IdEntrevista { get; set; }
        public int IdOferente { get; set; }
        public string NombreOferente { get; set; } = string.Empty;
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public DateTime FechaEntrevista { get; set; }
        public string Estado { get; set; } = "Pendiente";
    }
}
