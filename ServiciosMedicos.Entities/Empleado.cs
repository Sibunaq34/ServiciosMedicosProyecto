namespace ServiciosMedicos.Entities
{
    public class Empleado
    {
        public int IdEmpleado { get; set; }

        public string NumeroEmpleado { get; set; } = string.Empty;

        public string NombreCompleto { get; set; } = string.Empty;

        public string Identificacion { get; set; } = string.Empty;

        public int IdPuesto { get; set; }

        public string Estado { get; set; } = string.Empty;
    }
}