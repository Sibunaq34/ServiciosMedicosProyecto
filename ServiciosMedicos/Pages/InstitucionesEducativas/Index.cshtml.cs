using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.InstitucionesEducativas
{
    // Persona C - Kenneth: Listado paginado y eliminacion para GEN5.
    public class IndexModel : PageModel
    {
        private const int TamanoPagina = 10;
        private readonly IInstitucionEducativaService _service;

        public IndexModel(
            IInstitucionEducativaService service)
        {
            _service = service;
        }

        public IReadOnlyList<InstitucionEducativa> Instituciones { get; set; } =
            Array.Empty<InstitucionEducativa>();

        public int Pagina { get; set; } = 1;

        public bool HaySiguientePagina =>
            Instituciones.Count == TamanoPagina;

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
                Instituciones =
                    await _service.ListarAsync(
                        Pagina,
                        TamanoPagina,
                        idUsuario.Value);
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
                Instituciones = Array.Empty<InstitucionEducativa>();
            }
            catch
            {
                MensajeError =
                    "Ocurrió un error al consultar las instituciones educativas.";

                Instituciones = Array.Empty<InstitucionEducativa>();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(
            int idInstitucion,
            int pagina = 1)
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            try
            {
                await _service.EliminarAsync(
                    idInstitucion,
                    idUsuario.Value);

                MensajeExito =
                    "La institución educativa fue eliminada correctamente.";
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
            }
            catch
            {
                MensajeError =
                    "Ocurrió un error al eliminar la institución educativa.";
            }

            return RedirectToPage(
                "./Index",
                new { pagina });
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
