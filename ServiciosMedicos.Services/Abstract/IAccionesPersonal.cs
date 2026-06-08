

using Servicios_Medicos.Entities;

namespace ServiciosMedicos.Services.Abstract
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