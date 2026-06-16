using Dapper;
using Servicios_Medicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository;

public class PantallasRepository
{
    private readonly IDbConnectionFactory _db;

    public PantallasRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Pantalla>> Listar()
    {
        using var connection = _db.CreateConnection();

        var pantallas = await connection.QueryAsync(
            "CALL sp_Pantallas_Listar();"
        );

        return pantallas.Select(p => new Pantalla
        {
            IdPantalla = p.id_pantalla,
            NombrePantalla = p.nombre_pantalla,
            Activo = p.activo == 1
        });
    }

    public async Task<Pantalla?> ObtenerPorId(int idPantalla)
    {
        using var connection = _db.CreateConnection();

        var pantalla = await connection.QueryFirstOrDefaultAsync(
            "CALL sp_Pantallas_ObtenerPorId(@p_idPantalla);",
            new { p_idPantalla = idPantalla }
        );

        if (pantalla == null)
            return null;

        return new Pantalla
        {
            IdPantalla = pantalla.id_pantalla,
            NombrePantalla = pantalla.nombre_pantalla,
            Activo = pantalla.activo == 1
        };
    }

    public async Task Crear(Pantalla pantalla)
    {
        using var connection = _db.CreateConnection();

        await connection.ExecuteAsync(
            "sp_Pantallas_Crear",
            new
            {
                p_nombrePantalla = pantalla.NombrePantalla
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task Actualizar(Pantalla pantalla)
    {
        using var connection = _db.CreateConnection();

        await connection.ExecuteAsync(
            "sp_Pantallas_Actualizar",
            new
            {
                p_idPantalla = pantalla.IdPantalla,
                p_nombrePantalla = pantalla.NombrePantalla
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task Eliminar(int idPantalla)
    {
        using var connection = _db.CreateConnection();

        await connection.ExecuteAsync(
            "sp_Pantallas_Eliminar",
            new
            {
                p_idPantalla = idPantalla
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Rol>> ListarRoles()
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<Rol>(
            "CALL sp_Roles_Listar();"
        );
    }

    public async Task<IEnumerable<int>> ListarRolesPorPantalla(int idPantalla)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<int>(
            "CALL sp_PantallaRol_ListarPorPantalla(@p_idPantalla);",
            new { p_idPantalla = idPantalla }
        );
    }

    public async Task GuardarRolesPantalla(int idPantalla, List<int> rolesSeleccionados)
    {
        using var connection = _db.CreateConnection();

        await connection.ExecuteAsync(
            "sp_PantallaRol_EliminarPorPantalla",
            new { p_idPantalla = idPantalla },
            commandType: CommandType.StoredProcedure);

        foreach (var idRol in rolesSeleccionados)
        {
            await connection.ExecuteAsync(
                "sp_PantallaRol_Asignar",
                new
                {
                    p_idPantalla = idPantalla,
                    p_idRol = idRol
                },
                commandType: CommandType.StoredProcedure);
        }
    }
    public async Task<IEnumerable<string>> ListarNombresPantallasPorRol(int idRol)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<string>(
            "CALL sp_Pantallas_ListarPorRol(@p_idRol);",
            new { p_idRol = idRol }
        );
    }
}