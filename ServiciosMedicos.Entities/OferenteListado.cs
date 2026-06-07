namespace Servicios_Medicos.Entities
{
    // Persona C - Kenneth: DTO de listado para OFE1.
    public class OferenteListado
    {
        public int IdOferente { get; set; }

        public int IdPersona { get; set; }

        public string Identificacion { get; set; } = string.Empty;

        public string TipoIdentificacion { get; set; } = string.Empty;

        public string NombreCompleto { get; set; } = string.Empty;

        public DateTime FechaNacimiento { get; set; }

        public DateTime FechaRegistro { get; set; }

        public IReadOnlyList<string> Correos { get; set; } =
            Array.Empty<string>();

        public IReadOnlyList<string> Telefonos { get; set; } =
            Array.Empty<string>();

        public IReadOnlyList<Concurso> Concursos { get; set; } =
            Array.Empty<Concurso>();
    }
}
