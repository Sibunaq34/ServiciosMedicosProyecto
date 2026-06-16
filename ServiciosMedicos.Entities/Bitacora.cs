using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosMedicos.Entities
{
    public class Bitacora
    {
        public int IdBitacora { get; set; }

        public DateTime FechaBitacora { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public string Accion { get; set; } = string.Empty;

        
        public string DescripcionAccion { get; set; } = string.Empty;

        // Filtros
        public string? FiltroUsuario { get; set; }

        public string? FiltroDescripcion { get; set; }
    }
}