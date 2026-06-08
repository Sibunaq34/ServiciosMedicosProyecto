using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Servicios_Medicos.Entities;

namespace Servicios_Medicos.Services.Abstract;

public interface IUsuariosAdmin
{
    Task<IEnumerable<UsuarioAdmin>> Listar();
    Task<UsuarioAdmin?> ObtenerPorId(int idUsuario);
    Task Crear(UsuarioAdmin usuario);
    Task Actualizar(UsuarioAdmin usuario);
    Task CambiarEstado(int idUsuario, bool activo);
    Task Eliminar(int idUsuario);
}