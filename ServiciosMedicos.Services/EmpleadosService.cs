using ServiciosMedicos.Services.Abstract;
using Servicios_Medicos.Repository;
using ServiciosMedicos.Entities;

namespace ServiciosMedicos.Services
{
    public class EmpleadosService : IEmpleados
    {
        private readonly EmpleadosRepository _empleadosBD;

        public EmpleadosService(
            EmpleadosRepository empleadosBD)
        {
            _empleadosBD = empleadosBD;
        }

        public async Task<IEnumerable<OferenteCombo>> ListarOferentes()
        {
            return await _empleadosBD.ListarOferentes();
        }

        public async Task<IEnumerable<Puesto>> ListarPuestos()
        {
            return await _empleadosBD.ListarPuestos();
        }

        public async Task<IEnumerable<EmpleadoCombo>> ListarEmpleados()
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