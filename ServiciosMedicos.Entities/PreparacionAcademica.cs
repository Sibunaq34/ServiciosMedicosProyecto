namespace Servicios_Medicos.Entities
{
    // Persona C - Kenneth: Entidad para OFE3 Preparacion academica.
    public class PreparacionAcademica
    {
        public int IdPreparacion { get; set; }

        public int IdOferente { get; set; }

        public int IdInstitucion { get; set; }

        public string CodigoInstitucion { get; set; } = string.Empty;

        public string NombreInstitucion { get; set; } = string.Empty;

        public string Titulo { get; set; } = string.Empty;

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }
    }
}
