using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;

namespace Servicios_Medicos.Services
{
    // Persona C - Kenneth: Servicio de aplicacion para GEN5.
    public class InstitucionEducativaService : IInstitucionEducativaService
    {
        private readonly InstitucionEducativaRepository _repository;

        public InstitucionEducativaService(
            InstitucionEducativaRepository repository)
        {
            _repository = repository;
        }

        public Task<IReadOnlyList<InstitucionEducativa>> ListarAsync(
            int pagina,
            int tamanoPagina,
            int idUsuario)
        {
            return _repository.ListarAsync(
                pagina,
                tamanoPagina,
                idUsuario);
        }

        public Task<InstitucionEducativa?> ObtenerAsync(
            int idInstitucion,
            int idUsuario)
        {
            return _repository.ObtenerAsync(
                idInstitucion,
                idUsuario);
        }

        public Task<int> CrearAsync(
            InstitucionEducativa institucion,
            int idUsuario)
        {
            Normalizar(institucion);

            return _repository.CrearAsync(
                institucion,
                idUsuario);
        }

        public Task ActualizarAsync(
            InstitucionEducativa institucion,
            int idUsuario)
        {
            Normalizar(institucion);

            return _repository.ActualizarAsync(
                institucion,
                idUsuario);
        }

        public Task EliminarAsync(
            int idInstitucion,
            int idUsuario)
        {
            return _repository.EliminarAsync(
                idInstitucion,
                idUsuario);
        }

        private static void Normalizar(
            InstitucionEducativa institucion)
        {
            institucion.Codigo =
                institucion.Codigo.Trim();

            institucion.Nombre =
                institucion.Nombre.Trim();
        }
    }
}
