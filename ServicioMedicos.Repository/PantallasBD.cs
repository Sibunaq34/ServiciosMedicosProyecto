using Dapper;
using Servicios_Medicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository;

public class PantallasBD
{
    private readonly IDbConnectionFactory _db;

    public PantallasBD(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Pantalla>> Listar()
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<Pantalla>(
            "sp_Pantallas_Listar",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Pantalla?> ObtenerPorId(int idPantalla)
    {
        using var connection = _db.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Pantalla>(
            "sp_Pantallas_ObtenerPorId",
            new { p_id_pantalla = idPantalla },
            commandType: CommandType.StoredProcedure);
    }

    public async Task Crear(Pantalla pantalla)
    {
        using var connection = _db.CreateConnection();

        await connection.ExecuteAsync(
            "sp_Pantallas_Crear",
            new { p_nombre_pantalla = pantalla.NombrePantalla },
            commandType: CommandType.StoredProcedure);
    }

    public async Task Actualizar(Pantalla pantalla)
    {
        using var connection = _db.CreateConnection();

        await connection.ExecuteAsync(
            "sp_Pantallas_Actualizar",
            new
            {
                p_id_pantalla = pantalla.IdPantalla,
                p_nombre_pantalla = pantalla.NombrePantalla
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task Eliminar(int idPantalla)
    {
        using var connection = _db.CreateConnection();

        await connection.ExecuteAsync(
            "sp_Pantallas_Eliminar",
            new { p_id_pantalla = idPantalla },
            commandType: CommandType.StoredProcedure);
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
