using ServiciosMedicos.Services.Abstract;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Entities;

namespace ServiciosMedicos.Services
{
    public class EmpleadosService : IEmpleados
    {
        private readonly EmpleadosBD _empleadosBD;

        public EmpleadosService(
            EmpleadosBD empleadosBD)
        {
            _empleadosBD = empleadosBD;
        }

        public async Task<IEnumerable<OferenteCombo>>ListarOferentes()
        {
            return await _empleadosBD.ListarOferentes();
        }

        public async Task<IEnumerable<Puesto>>ListarPuestos()
        {
            return await _empleadosBD.ListarPuestos();
        }

        public async Task<IEnumerable<EmpleadoCombo>>ListarEmpleados()
        {
            return await _empleadosBD.ListarEmpleados();
        }

        public async Task<bool> ContratarEmpleado(EmpleadoContratacion empleado)
        {
            return await _empleadosBD
                .ContratarEmpleado(empleado);
        }

    }
}