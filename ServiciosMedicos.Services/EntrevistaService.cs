using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;

namespace Servicios_Medicos.Services
{
    public class EntrevistaService
    {
        private readonly EntrevistaRepository _repo;
        private readonly BitacoraRepository _bitacora;
        private const int TamanoPagina = 10;

        public EntrevistaService(EntrevistaRepository repo, BitacoraRepository bitacora)
        {
            _repo = repo;
            _bitacora = bitacora;
        }

        public async Task<(IEnumerable<Entrevista> Items, int Total)> ObtenerPaginado(int pagina, int idUsuario)
        {
            try
            {
                var resultado = await _repo.ObtenerPaginado(pagina, TamanoPagina);
                await _bitacora.Registrar(idUsuario, "CONSULTA", new
                {
                    tabla = "entrevistas",
                    pagina
                });
                return resultado;
            }
            catch (Exception ex)
            {
                await _bitacora.Registrar(idUsuario, "ERROR", new { mensaje = ex.Message });
                return (Enumerable.Empty<Entrevista>(), 0);
            }
        }

        public async Task<Entrevista?> ObtenerPorId(int id)
        {
            return await _repo.ObtenerPorId(id);
        }

        public async Task<IEnumerable<OferenteResumen>> ObtenerOferentes()
        {
            return await _repo.ObtenerOferentes();
        }

        public async Task<IEnumerable<EmpleadoResumen>> ObtenerEmpleados()
        {
            return await _repo.ObtenerEmpleados();
        }

        public async Task<(bool Exito, string Mensaje)> Agendar(Entrevista entrevista, int idUsuario)
        {
            try
            {
                var newId = await _repo.Insertar(entrevista);
                entrevista.IdEntrevista = newId;
                await _bitacora.Registrar(idUsuario, "INSERCION", new
                {
                    tabla = "entrevistas",
                    registro = entrevista
                });
                return (true, "Entrevista agendada correctamente.");
            }
            catch (Exception ex)
            {
                await _bitacora.Registrar(idUsuario, "ERROR", new { mensaje = ex.Message });
                return (false, "Ocurrió un error al agendar la entrevista.");
            }
        }

        public async Task<(bool Exito, string Mensaje)> Actualizar(Entrevista antes, Entrevista despues, int idUsuario)
        {
            try
            {
                await _repo.Actualizar(despues);
                await _bitacora.Registrar(idUsuario, "ACTUALIZACION", new
                {
                    tabla = "entrevistas",
                    antes,
                    despues
                });
                return (true, "Entrevista actualizada correctamente.");
            }
            catch (Exception ex)
            {
                await _bitacora.Registrar(idUsuario, "ERROR", new { mensaje = ex.Message });
                return (false, "Ocurrió un error al actualizar la entrevista.");
            }
        }

        public async Task<(bool Exito, string Mensaje)> MarcarRealizada(int id, int idUsuario)
        {
            try
            {
                await _repo.MarcarRealizada(id);
                await _bitacora.Registrar(idUsuario, "ACTUALIZACION", new
                {
                    tabla = "entrevistas",
                    id_entrevista = id,
                    estado = "Realizada"
                });
                return (true, "Entrevista marcada como realizada.");
            }
            catch (Exception ex)
            {
                await _bitacora.Registrar(idUsuario, "ERROR", new { mensaje = ex.Message });
                return (false, "Ocurrió un error al actualizar el estado.");
            }
        }

        public async Task<(bool Exito, string Mensaje)> Eliminar(int id, int idUsuario)
        {
            try
            {
                var registro = await _repo.ObtenerPorId(id);
                await _repo.Eliminar(id);
                await _bitacora.Registrar(idUsuario, "ELIMINACION", new
                {
                    tabla = "entrevistas",
                    registro
                });
                return (true, "Entrevista eliminada correctamente.");
            }
            catch (Exception ex)
            {
                await _bitacora.Registrar(idUsuario, "ERROR", new { mensaje = ex.Message });
                return (false, "Ocurrió un error al eliminar la entrevista.");
            }
        }
    }
}