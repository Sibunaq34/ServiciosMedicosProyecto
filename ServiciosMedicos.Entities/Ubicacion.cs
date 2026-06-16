namespace ServiciosMedicos.Entities
{
    public class Ubicacion
    {
        public string Provincia { get; set; } = string.Empty;

        public string Canton { get; set; } = string.Empty;

        public string Distrito { get; set; } = string.Empty;

        public int idusuario { get; set; }
    }
}