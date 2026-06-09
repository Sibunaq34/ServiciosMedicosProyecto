using ServiciosMedicos.Entities;
using Servicios_Medicos.Repository;
using ServiciosMedicos.Services.Abstract;


namespace Servicios_Medicos.Services
{
    public class PuestosService : IPuestos
    {
        private readonly PuestosBD _puestosBD;

        public PuestosService(PuestosBD puestosBD)
        {
            _puestosBD = puestosBD;
        }

        public async Task<IEnumerable<Puesto>> ListarPuestos()
        {
            return await _puestosBD.ListarPuestos();
        }

        public async Task<Puesto?> ObtenerPuesto(int idPuesto)
        {
            return await _puestosBD.ObtenerPuesto(idPuesto);
        }

        public async Task<bool> InsertarPuesto(Puesto puesto)
        {
            return await _puestosBD.InsertarPuesto(puesto);
        }

        public async Task<bool> ActualizarPuesto(Puesto puesto)
        {
            return await _puestosBD.ActualizarPuesto(puesto);
        }

        public async Task<bool> EliminarPuesto(int idPuesto)
        {
            return await _puestosBD.EliminarPuesto(idPuesto);
        }
    }
}