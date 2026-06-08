using Servicios_Medicos.Entities;

namespace Servicios_Medicos.Services.Abstract
{
    public interface IParametro
    {
        Task<IEnumerable<ParametroEntidad>> Listar();

        Task<ParametroEntidad?> ObtenerPorId(int id);

        Task<bool> Insertar(
            ParametroEntidad parametro);

        Task<bool> Actualizar(
            ParametroEntidad parametro);

        Task<bool> Eliminar(int id);
    }
}