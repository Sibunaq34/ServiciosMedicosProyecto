using ServiciosMedicos.Entities;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;

namespace Servicios_Medicos.Services
{
    public class Bitacora : IBitacora
    {
        private readonly BitacoraBD _bitacoraBD;

        public Bitacora(BitacoraBD bitacoraBD)
        {
            _bitacoraBD = bitacoraBD;
        }

        public async Task<IEnumerable<BitacoraEntidad>> ConsultarBitacoras(
            string? usuario,
            string? descripcion)
        {
            return await _bitacoraBD
                .ConsultarBitacoras(
                    usuario,
                    descripcion);
        }
    }
}