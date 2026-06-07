using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios_Medicos.Entities;

public class Pantalla
{
    public int IdPantalla { get; set; }
    public string NombrePantalla { get; set; } = string.Empty;
    public bool Activo { get; set; }
}