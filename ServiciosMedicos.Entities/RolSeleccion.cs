using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios_Medicos.Entities
{
    public class RolSeleccion
    {
        public int IdRol { get; set; }

        public string NombrePermiso { get; set; } = string.Empty;

        public bool Seleccionado { get; set; }
    }
}