using ServiciosMedicos.Entities;

namespace ServiciosMedicos.Services.Abstract
{
    public interface IEmpleados
    {
        Task<IEnumerable<OferenteCombo>>
            ListarOferentes();

        Task<IEnumerable<Puesto>>
            ListarPuestos();

        Task<IEnumerable<EmpleadoCombo>>
            ListarEmpleados();

        Task<bool>
            ContratarEmpleado(
            EmpleadoContratacion empleado);
    }
}