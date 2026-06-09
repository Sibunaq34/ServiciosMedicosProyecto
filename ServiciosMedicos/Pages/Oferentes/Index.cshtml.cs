using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Oferentes
{
    // Persona C - Kenneth: Listado paginado y eliminacion para OFE1.
    public class IndexModel : PageModel
    {
        private const int TamanoPagina = 10;
        private readonly IOferenteService _service;

        public IndexModel(
            IOferenteService service)
        {
            _service = service;
        }

        public IReadOnlyList<OferenteListado> Oferentes { get; set; } =
            Array.Empty<OferenteListado>();

        public int Pagina { get; set; } = 1;

        public bool HaySiguientePagina =>
            Oferentes.Count == TamanoPagina;

        [TempData]
        public string? MensajeExito { get; set; }

        [TempData]
        public string? MensajeError { get; set; }

        public async Task<IActionResult> OnGetAsync(
            int pagina = 1)
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            Pagina = pagina < 1 ? 1 : pagina;

            try
            {
                Oferentes =
                    await _service.ListarAsync(
                        Pagina,
                        TamanoPagina,
                        idUsuario.Value);
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
                Oferentes = Array.Empty<OferenteListado>();
            }
            catch
            {
                MensajeError =
                    "Ocurrio un error al consultar los oferentes.";

                Oferentes = Array.Empty<OferenteListado>();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(
            int idOferente,
            int pagina = 1)
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            try
            {
                await _service.EliminarAsync(
                    idOferente,
                    idUsuario.Value);

                MensajeExito =
                    "El oferente fue eliminado correctamente.";
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
            }
            catch
            {
                MensajeError =
                    "Ocurrio un error al eliminar el oferente.";
            }

            return RedirectToPage(
                "./Index",
                new { pagina });
        }

        public string ObtenerTextoTipoIdentificacion(
            string tipoIdentificacion)
        {
            return tipoIdentificacion switch
            {
                "CedulaIdentidad" => "Cedula de identidad",
                "DIMEX" => "DIMEX",
                "Pasaporte" => "Pasaporte",
                _ => tipoIdentificacion
            };
        }

        private int? ObtenerIdUsuario()
        {
            return HttpContext.Session.GetInt32("IdUsuario");
        }

        private IActionResult RedirigirALogin()
        {
            return RedirectToPage(
                "/LogIn",
                new { expirada = true });
        }
    }
}
