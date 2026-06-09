namespace Servicios_Medicos.Entities
{
    public class ExperienciaLaboral
    {
        public int IdExperiencia { get; set; }
        public int IdOferente { get; set; }
        public string NombreEmpresa { get; set; } = string.Empty;
        public string PuestoDesempenado { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
