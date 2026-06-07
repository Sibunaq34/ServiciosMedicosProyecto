using ServiciosMedicos.Entities;

namespace ServiciosMedicos.Services.Abstract
{
    public interface IRequisitos
    {
        Task<IEnumerable<RequisitoPuesto>> ListarRequisitos(int idPuesto);

        Task<bool> InsertarRequisito(RequisitoPuesto requisito);

        Task<bool> ActualizarRequisito(RequisitoPuesto requisito);

        Task<bool> EliminarRequisito(int idRequisito);
    }
}