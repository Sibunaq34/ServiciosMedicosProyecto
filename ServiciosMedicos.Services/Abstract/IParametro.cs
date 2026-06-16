using Servicios_Medicos.Entities;

public interface IParametro
{
    Task<IReadOnlyList<Parametros>> Listar(int pagina, int tamanoPagina);

    Task<Parametros?> ObtenerPorId(int id);

    Task<bool> Insertar(Parametros parametro, int idUsuario);

    Task<bool> Actualizar(
        Parametros parametro,
        int idUsuario);

    Task<bool> Eliminar(
        int id,
        int idUsuario);

    Task<Parametros?> ObtenerPorCodigo(
        string codigo);

    Task<string?> ObtenerValor(
        string codigo);
}