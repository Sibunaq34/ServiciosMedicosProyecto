using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;

namespace Servicios_Medicos.Services
{
    public class Parametro : IParametro
    {
        private readonly ParametroBD _parametroBD;

        public Parametro(
            ParametroBD parametroBD)
        {
            _parametroBD = parametroBD;
        }

        public Task<IEnumerable<ParametroEntidad>>
            Listar()
        {
            return _parametroBD.Listar();
        }

        public Task<ParametroEntidad?>
            ObtenerPorId(int id)
        {
            return _parametroBD.ObtenerPorId(id);
        }

        public Task<bool>
            Insertar(ParametroEntidad parametro)
        {
            return _parametroBD.Insertar(parametro);
        }

        public Task<bool>
            Actualizar(ParametroEntidad parametro)
        {
            return _parametroBD.Actualizar(parametro);
        }

        public Task<bool>
            Eliminar(int id)
        {
            return _parametroBD.Eliminar(id);
        }
        public Task<ParametroEntidad?>
            ObtenerPorCodigo(string codigo)
        {
            return _parametroBD.ObtenerPorCodigo(codigo);
        }

        public async Task<string?>
            ObtenerValor(string codigo)
        {
            var parametro =
                await _parametroBD
                    .ObtenerPorCodigo(codigo);

            return parametro?.Valor;
        }
    }
}