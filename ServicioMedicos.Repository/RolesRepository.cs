using Dapper;
using Servicios_Medicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository;

public class RolesRepository
{
    private readonly IDbConnectionFactory _db;

    public RolesRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Rol>> Listar()
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<Rol>(
            "sp_Roles_Listar",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Rol?> ObtenerPorId(int idRol)
    {
        using var connection = _db.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Rol>(
            "sp_Roles_ObtenerPorId",
            new { p_idRol = idRol },
            commandType: CommandType.StoredProcedure);
    }

    public async Task Crear(Rol rol)
    {
        using var connection = _db.CreateConnection();

        await connection.ExecuteAsync(
            "sp_Roles_Crear",
            new { p_nombrePermiso = rol.NombrePermiso },
            commandType: CommandType.StoredProcedure);
    }

    public async Task Actualizar(Rol rol)
    {
        using var connection = _db.CreateConnection();

        await connection.ExecuteAsync(
            "sp_Roles_Actualizar",
            new
            {
                p_idRol = rol.IdRol,
                p_nombrePermiso = rol.NombrePermiso
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task Eliminar(int idRol)
    {
        using var connection = _db.CreateConnection();

        await connection.ExecuteAsync(
            "sp_Roles_Eliminar",
            new { p_idRol = idRol },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Pantalla>> ListarPantallasPorRol(int idRol)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<Pantalla>(
            "sp_RolPantalla_ListarPorRol",
            new { p_idRol = idRol },
            commandType: CommandType.StoredProcedure);
    }

    public async Task GuardarPantallasRol(int idRol, List<int> pantallasSeleccionadas)
    {
        using var connection = _db.CreateConnection();

        await connection.ExecuteAsync(
            "sp_RolPantalla_EliminarPorRol",
            new { p_idRol = idRol },
            commandType: CommandType.StoredProcedure);

        foreach (var idPantalla in pantallasSeleccionadas)
        {
            await connection.ExecuteAsync(
                "sp_RolPantalla_Asignar",
                new
                {
                    p_idRol = idRol,
                    p_idPantalla = idPantalla
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}