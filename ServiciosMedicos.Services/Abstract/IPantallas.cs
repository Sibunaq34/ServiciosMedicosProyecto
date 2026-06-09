
using Servicios_Medicos.Entities;

namespace Servicios_Medicos.Services.Abstract
{
    public interface IPantallas
    {
        Task<IEnumerable<Pantalla>> Listar();

        Task<Pantalla?> ObtenerPorId(int idPantalla);

        Task Crear(Pantalla pantalla);

        Task Actualizar(Pantalla pantalla);

        Task Eliminar(int idPantalla);

    }
}