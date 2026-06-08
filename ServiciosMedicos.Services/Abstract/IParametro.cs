using Servicios_Medicos.Entities;

public interface IParametro
{
    Task<IEnumerable<ParametroEntidad>> Listar();

    Task<ParametroEntidad?> ObtenerPorId(int id);

    Task<bool> Insertar(ParametroEntidad parametro, int idUsuario);

    Task<bool> Actualizar(
        ParametroEntidad parametro,
        int idUsuario);

    Task<bool> Eliminar(
        int id,
        int idUsuario);

    Task<ParametroEntidad?> ObtenerPorCodigo(
        string codigo);

    Task<string?> ObtenerValor(
        string codigo);
}