using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;

namespace Servicios_Medicos.Services
{
    // Persona C - Kenneth: Servicio de aplicacion para OFE1.
    public class OferenteService : IOferenteService
    {
        private readonly OferenteRepository _repository;

        public OferenteService(
            OferenteRepository repository)
        {
            _repository = repository;
        }

        public Task<IReadOnlyList<OferenteListado>> ListarAsync(
            int pagina,
            int tamanoPagina,
            int idUsuario)
        {
            return _repository.ListarAsync(
                pagina,
                tamanoPagina,
                idUsuario);
        }

        public Task<OferenteDetalle?> ObtenerAsync(
            int idOferente,
            int idUsuario)
        {
            return _repository.ObtenerAsync(
                idOferente,
                idUsuario);
        }

        public Task<int> CrearAsync(
            Oferente oferente,
            int idUsuario)
        {
            Normalizar(oferente);

            return _repository.CrearAsync(
                oferente,
                idUsuario);
        }

        public Task ActualizarAsync(
            Oferente oferente,
            int idUsuario)
        {
            Normalizar(oferente);

            return _repository.ActualizarAsync(
                oferente,
                idUsuario);
        }

        public Task EliminarAsync(
            int idOferente,
            int idUsuario)
        {
            return _repository.EliminarAsync(
                idOferente,
                idUsuario);
        }

        private static void Normalizar(
            Oferente oferente)
        {
            oferente.Identificacion =
                oferente.Identificacion.Trim();

            oferente.TipoIdentificacion =
                oferente.TipoIdentificacion.Trim();

            oferente.NombreCompleto =
                oferente.NombreCompleto.Trim();

            oferente.Correos =
                oferente.Correos
                    .Select(correo => correo.Trim())
                    .Where(correo => !string.IsNullOrWhiteSpace(correo))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

            oferente.Telefonos =
                oferente.Telefonos
                    .Select(telefono => telefono.Trim())
                    .Where(telefono => !string.IsNullOrWhiteSpace(telefono))
                    .Distinct()
                    .ToList();

            oferente.ConcursosIds =
                oferente.ConcursosIds
                    .Where(id => id > 0)
                    .Distinct()
                    .ToList();
        }
    }
}
