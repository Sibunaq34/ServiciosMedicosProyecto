using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;
using System.Text.RegularExpressions;

namespace Servicios_Medicos.Services;

public class Roles : IRoles
{
    private readonly RolesBD _rolesBD;

    public Roles(RolesBD rolesBD)
    {
        _rolesBD = rolesBD;
    }

    public Task<IEnumerable<Rol>> Listar() => _rolesBD.Listar();

    public Task<Rol?> ObtenerPorId(int idRol) => _rolesBD.ObtenerPorId(idRol);

    public async Task Crear(Rol rol)
    {
        Validar(rol);
        await _rolesBD.Crear(rol);
    }

    public async Task Actualizar(Rol rol)
    {
        Validar(rol);
        await _rolesBD.Actualizar(rol);
    }

    public Task Eliminar(int idRol) => _rolesBD.Eliminar(idRol);

    private static void Validar(Rol rol)
    {
        if (string.IsNullOrWhiteSpace(rol.NombrePermiso))
            throw new Exception("El nombre del rol es obligatorio.");

        if (rol.NombrePermiso.Length > 40)
            throw new Exception("El nombre del rol no debe superar 40 caracteres.");

        if (!Regex.IsMatch(rol.NombrePermiso, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            throw new Exception("El nombre del rol solo debe tener letras y espacios.");
    }
    public Task<IEnumerable<Pantalla>> ListarPantallasPorRol(int idRol)
    {
        return _rolesBD.ListarPantallasPorRol(idRol);
    }

    public Task GuardarPantallasRol(
        int idRol,
        List<int> pantallasSeleccionadas)
    {
        return _rolesBD.GuardarPantallasRol(
            idRol,
            pantallasSeleccionadas);
    }
}