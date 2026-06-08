using Servicios_Medicos.Entities;

public interface IParametro
{
    Task<IEnumerable<ParametroEntidad>> Listar();

    Task<ParametroEntidad?> ObtenerPorId(int id);

    Task<ParametroEntidad?> ObtenerPorCodigo(string codigo);

    Task<string?> ObtenerValor(string codigo);

    Task<bool> Insertar(ParametroEntidad parametro);

    Task<bool> Actualizar(ParametroEntidad parametro);

    Task<bool> Eliminar(int id);
}