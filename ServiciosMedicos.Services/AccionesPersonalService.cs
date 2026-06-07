using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;
using ServiciosMedicos.Services.Abstract;

namespace Servicios_Medicos.Services
{
    public class AccionesPersonalService
        : IAccionesPersonal
    {
        private readonly AccionesPersonalBD _accionesBD;

        public AccionesPersonalService(
            AccionesPersonalBD accionesBD)
        {
            _accionesBD = accionesBD;
        }

        public async Task<IEnumerable<AccionPersonal>>
            ListarAcciones()
        {
            return await _accionesBD.ListarAcciones();
        }

        public async Task<bool>
            InsertarAccion(
            AccionPersonal accion)
        {
            return await _accionesBD
                .InsertarAccion(accion);
        }
    }
}