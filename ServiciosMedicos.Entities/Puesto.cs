
namespace Servicios_Medicos.Entities
{
    public class Puesto
    {
        public int IdPuesto { get; set; }

        public string CodigoPuesto { get; set; } = string.Empty;

        public string NombrePuesto { get; set; } = string.Empty;

        public decimal MontoSalario { get; set; }

        public int? IdPuestoJefac { get; set; }

        public string? Jefatura { get; set; }
    }
}