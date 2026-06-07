using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;

namespace Servicios_Medicos.Services
{
    public class AreaService
    {
        private readonly AreaRepository _repo;
        private readonly BitacoraRepository _bitacora;
        private const int TamanoPagina = 10;

        public AreaService(AreaRepository repo, BitacoraRepository bitacora)
        {
            _repo = repo;
            _bitacora = bitacora;
        }

        public async Task<(IEnumerable<Area> Items, int Total)> ObtenerPaginado(int pagina, int idUsuario)
        {
            try
            {
                var resultado = await _repo.ObtenerPaginado(pagina, TamanoPagina);
                await _bitacora.Registrar(idUsuario, "CONSULTA", new
                {
                    tabla = "admin_area",
                    pagina
                });
                return resultado;
            }
            catch (Exception ex)
            {
                await _bitacora.Registrar(idUsuario, "ERROR", new { mensaje = ex.Message });
                return (Enumerable.Empty<Area>(), 0);
            }
        }

        public async Task<Area?> ObtenerPorId(int id)
        {
            return await _repo.ObtenerPorId(id);
        }

        public async Task<IEnumerable<EmpleadoResumen>> ObtenerEmpleados()
        {
            return await _repo.ObtenerEmpleados();
        }

        public async Task<(bool Exito, string Mensaje)> Insertar(Area area, int idUsuario)
        {
            try
            {
                if (await _repo.CodigoExiste(area.CodigoArea, null))
                    return (false, "El código de área ya existe.");

                var newId = await _repo.Insertar(area);
                area.IdArea = newId;
                await _bitacora.Registrar(idUsuario, "INSERCION", new
                {
                    tabla = "admin_area",
                    registro = area
                });
                return (true, "Área registrada correctamente.");
            }
            catch (Exception ex)
            {
                await _bitacora.Registrar(idUsuario, "ERROR", new { mensaje = ex.Message });
                return (false, "Ocurrió un error al registrar el área.");
            }
        }

        public async Task<(bool Exito, string Mensaje)> Actualizar(Area antes, Area despues, int idUsuario)
        {
            try
            {
                if (await _repo.CodigoExiste(despues.CodigoArea, despues.IdArea))
                    return (false, "El código de área ya existe.");

                await _repo.Actualizar(despues);
                await _bitacora.Registrar(idUsuario, "ACTUALIZACION", new
                {
                    tabla = "admin_area",
                    antes,
                    despues
                });
                return (true, "Área actualizada correctamente.");
            }
            catch (Exception ex)
            {
                await _bitacora.Registrar(idUsuario, "ERROR", new { mensaje = ex.Message });
                return (false, "Ocurrió un error al actualizar el área.");
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
                    tabla = "admin_area",
                    registro
                });
                return (true, "Área eliminada correctamente.");
            }
            catch (Exception ex)
            {
                await _bitacora.Registrar(idUsuario, "ERROR", new { mensaje = ex.Message });
                return (false, "No se puede eliminar un registro con datos relacionados.");
            }
        }
    }
}
