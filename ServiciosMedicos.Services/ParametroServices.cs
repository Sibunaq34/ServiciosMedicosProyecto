using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;
using Microsoft.AspNetCore.Http;

namespace Servicios_Medicos.Services
{
    public class ParametroServices : IParametro
    {
        private readonly ParametroBD _parametroBD;
        private readonly BitacoraRepository _bitacora;

        public ParametroServices(ParametroBD parametroBD, BitacoraRepository bitacora)
        {
            _parametroBD = parametroBD;
            _bitacora = bitacora;
        }

        public async Task<IReadOnlyList<Parametros>> Listar(int pagina, int tamanoPagina)
        {
            var resultado = await _parametroBD.Listar(pagina, tamanoPagina);
            return resultado.ToList();
        }

        public Task<Parametros?>
            ObtenerPorId(int id)
        {
            return _parametroBD.ObtenerPorId(id);
        }

        public async Task<bool> Insertar(Parametros parametro, int idUsuario)
        {
            await _bitacora.Registrar(idUsuario, "INSERTADO", new
            {
                tabla = "Parametros"
            });
            return await _parametroBD.Insertar(parametro);
        }

        public async Task<bool> Actualizar(Parametros parametro, int idUsuario)
        {
            await _bitacora.Registrar(idUsuario, "EDITADO", new
            {
                tabla = "Parametros"
            });
            return await _parametroBD.Actualizar(parametro);
        }

        public async Task<bool> Eliminar(int id, int idUsuario) 
        {   

            await _bitacora.Registrar(idUsuario, "ELIMINACION", new
            {
                tabla = "Parametros"
            });
            return await _parametroBD.Eliminar(id);
        }
        public Task<Parametros?>ObtenerPorCodigo(string codigo)
        {
            return _parametroBD.ObtenerPorCodigo(codigo);
        }

        public async Task<string?> ObtenerValor(string codigo)
        {
            var parametro = await _parametroBD.ObtenerPorCodigo(codigo);
            return parametro?.Valor;
        }
    }
}