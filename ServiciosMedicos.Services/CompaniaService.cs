using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;

namespace Servicios_Medicos.Services
{
    public class CompaniaService
    {
        private readonly CompaniaRepository _repo;
        private readonly BitacoraRepository _bitacora;

        public CompaniaService(CompaniaRepository repo, BitacoraRepository bitacora)
        {
            _repo = repo;
            _bitacora = bitacora;
        }

        public async Task<(IEnumerable<Compania> Items, int Total)> ObtenerPaginado(int pagina, int idUsuario)
        {
            var resultado = await _repo.ObtenerPaginado(pagina);
            await _bitacora.Registrar(idUsuario, "CONSULTA", "El usuario consulta compañías");
            return resultado;
        }

        public async Task<Compania?> ObtenerPorId(int id)
        {
            return await _repo.ObtenerPorId(id);
        }

        public async Task<(bool Exito, string Mensaje)> Insertar(Compania compania, int idUsuario)
        {
            try
            {
                if (await _repo.CodigoExiste(compania.CodigoCompania))
                    return (false, "El código de compañía ya existe.");

                await _repo.Insertar(compania);
                await _bitacora.Registrar(idUsuario, "INSERCION", new { nuevo = compania }); //guarda el objeto nuevo como JSON
                return (true, "Compañía creada correctamente.");
            }
            catch (Exception ex)
            {
                await _bitacora.Registrar(idUsuario, "ERROR", new { error = ex.Message, operacion = "Insertar compañía" });
                return (false, "Ocurrió un error al guardar la compañía.");
            }
        }

        public async Task<(bool Exito, string Mensaje)> Actualizar(Compania antes, Compania despues, int idUsuario)
        {
            try
            {
                if (await _repo.CodigoExiste(despues.CodigoCompania, despues.IdCompania))
                    return (false, "El código de compañía ya existe.");

                await _repo.Actualizar(despues);
                await _bitacora.Registrar(idUsuario, "ACTUALIZACION", new { antes, despues });  //JSON
                return (true, "Compañía actualizada correctamente.");
            }
            catch (Exception ex)
            {
                await _bitacora.Registrar(idUsuario, "ERROR", new { error = ex.Message, operacion = "Actualizar compañía" });
                return (false, "Ocurrió un error al actualizar la compañía.");
            }
        }

        public async Task<(bool Exito, string Mensaje)> Eliminar(int id, int idUsuario)
        {
            try
            {
                var compania = await _repo.ObtenerPorId(id);
                if (compania == null)
                    return (false, "La compañía no existe.");

                if (await _repo.TieneRelaciones(id))
                    return (false, "No se puede eliminar un registro con datos relacionados.");

                await _repo.Eliminar(id);
                await _bitacora.Registrar(idUsuario, "ELIMINACION", new { eliminado = compania });  //JSON
                return (true, "Compañía eliminada correctamente.");
            }
            catch (Exception ex)
            {
                await _bitacora.Registrar(idUsuario, "ERROR", new { error = ex.Message, operacion = "Eliminar compañía" });
                return (false, "Ocurrió un error al eliminar la compañía.");
            }
        }
    }
}
