using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;

namespace Servicios_Medicos.Services
{
    public class ExperienciaLaboralService
    {
        private readonly ExperienciaLaboralRepository _repo;
        private readonly BitacoraRepository _bitacora;
        private const int TamanoPagina = 10;

        public ExperienciaLaboralService(ExperienciaLaboralRepository repo, BitacoraRepository bitacora)
        {
            _repo = repo;
            _bitacora = bitacora;
        }

        public async Task<(IEnumerable<ExperienciaLaboral> Items, int Total)> ObtenerPaginado(int idOferente, int pagina, int idUsuario)
        {
            try
            {
                var resultado = await _repo.ObtenerPaginado(idOferente, pagina, TamanoPagina);
                await _bitacora.Registrar(idUsuario, "CONSULTA", new
                {
                    tabla = "experiencia_laboral",
                    id_oferente = idOferente,
                    pagina
                });
                return resultado;
            }
            catch (Exception ex)
            {
                await _bitacora.Registrar(idUsuario, "ERROR", new { mensaje = ex.Message });
                return (Enumerable.Empty<ExperienciaLaboral>(), 0);
            }
        }

        public async Task<ExperienciaLaboral?> ObtenerPorId(int id)
        {
            return await _repo.ObtenerPorId(id);
        }

        public async Task<string?> ObtenerNombreOferente(int idOferente)
        {
            return await _repo.ObtenerNombreOferente(idOferente);
        }

        public async Task<(bool Exito, string Mensaje)> Insertar(ExperienciaLaboral exp, int idUsuario)
        {
            try
            {
                var newId = await _repo.Insertar(exp);
                exp.IdExperiencia = newId;
                await _bitacora.Registrar(idUsuario, "INSERCION", new
                {
                    tabla = "experiencia_laboral",
                    registro = exp
                });
                return (true, "Experiencia laboral registrada correctamente.");
            }
            catch (Exception ex)
            {
                await _bitacora.Registrar(idUsuario, "ERROR", new { mensaje = ex.Message });
                return (false, "Ocurrió un error al registrar la experiencia laboral.");
            }
        }

        public async Task<(bool Exito, string Mensaje)> Actualizar(ExperienciaLaboral antes, ExperienciaLaboral despues, int idUsuario)
        {
            try
            {
                await _repo.Actualizar(despues);
                await _bitacora.Registrar(idUsuario, "ACTUALIZACION", new
                {
                    tabla = "experiencia_laboral",
                    antes,
                    despues
                });
                return (true, "Experiencia laboral actualizada correctamente.");
            }
            catch (Exception ex)
            {
                await _bitacora.Registrar(idUsuario, "ERROR", new { mensaje = ex.Message });
                return (false, "Ocurrió un error al actualizar la experiencia laboral.");
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
                    tabla = "experiencia_laboral",
                    registro
                });
                return (true, "Experiencia laboral eliminada correctamente.");
            }
            catch (Exception ex)
            {
                await _bitacora.Registrar(idUsuario, "ERROR", new { mensaje = ex.Message });
                return (false, "Ocurrió un error al eliminar la experiencia laboral.");
            }
        }
    }
}
