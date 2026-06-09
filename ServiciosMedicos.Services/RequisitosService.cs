using ServiciosMedicos.Entities;
using Servicios_Medicos.Repository;
using ServiciosMedicos.Services.Abstract;

namespace ServiciosMedicos.Services
{
    public class RequisitosService : IRequisitos
    {
        private readonly RequisitosBD _requisitosBD;

        public RequisitosService(RequisitosBD requisitosBD)
        {
            _requisitosBD = requisitosBD;
        }

        public async Task<IEnumerable<RequisitoPuesto>> ListarRequisitos(int idPuesto)
        {
            return await _requisitosBD.ListarRequisitos(idPuesto);
        }

        public async Task<bool> InsertarRequisito(RequisitoPuesto requisito)
        {
            return await _requisitosBD.InsertarRequisito(requisito);
        }

        public async Task<bool> ActualizarRequisito(RequisitoPuesto requisito)
        {
            return await _requisitosBD.ActualizarRequisito(requisito);
        }

        public async Task<bool> EliminarRequisito(int idRequisito)
        {
            return await _requisitosBD.EliminarRequisito(idRequisito);
        }
    }
}
