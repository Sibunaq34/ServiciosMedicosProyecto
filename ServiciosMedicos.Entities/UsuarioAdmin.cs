using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios_Medicos.Entities
{
    public class UsuarioAdmin
    {
        public int IdUsuario { get; set; }

        public string UsuarioNombre { get; set; } = string.Empty;

        public string NombreCompleto { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string Contrasena { get; set; } = string.Empty;

        public bool Activo { get; set; }

        public string Estado { get; set; } = "Activo";

        public int? IdRol { get; set; }

        public string? NombrePermiso { get; set; }
    }
}