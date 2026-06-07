

using Servicios_Medicos.Entities;

namespace Servicios_Medicos.Services.Abstract
{
    public interface IAccionesPersonal
    {
        Task<IEnumerable<AccionPersonal>>
            ListarAcciones();

        Task<bool>
            InsertarAccion(
            AccionPersonal accion);
    }
}