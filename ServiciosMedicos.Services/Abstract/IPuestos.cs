using ServiciosMedicos.Entities;

namespace ServiciosMedicos.Services.Abstract
{
    public interface IPuestos
    {
        Task<IEnumerable<Puesto>> ListarPuestos();

        Task<bool> InsertarPuesto(Puesto puesto);

        Task<Puesto?> ObtenerPuesto(int idPuesto);

        Task<bool> ActualizarPuesto(Puesto puesto);

        Task<bool> EliminarPuesto(int idPuesto);
    }
}
