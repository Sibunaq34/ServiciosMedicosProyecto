namespace Servicios_Medicos.Entities
{
    public class AccionPersonal
    {
        public int IdAccion { get; set; }

        public string CodigoAccion { get; set; } = string.Empty;

        public DateTime FechaAccion { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public int IdEmpleado { get; set; }

        public int IdJefatura { get; set; }

        public string Empleado { get; set; } = string.Empty;

        public string Jefatura { get; set; } = string.Empty;
    }
}