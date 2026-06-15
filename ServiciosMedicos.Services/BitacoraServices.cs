using ServiciosMedicos.Entities;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;

namespace Servicios_Medicos.Services
{
    public class BitacoraServices : IBitacora
    {
        private readonly BitacoraConsultaRepository _bitacoraBD;

        public BitacoraServices(BitacoraConsultaRepository bitacoraBD)
        {
            _bitacoraBD = bitacoraBD;
        }

        public async Task<IEnumerable<ServiciosMedicos.Entities.Bitacora>> ConsultarBitacoras(
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