namespace Servicios_Medicos.Entities
{
    // Persona C - Kenneth: Entidad para OFE2 Concursos.
    public class Concurso
    {
        public int IdConcurso { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public string Estado { get; set; } = "Vigente";
    }
}
