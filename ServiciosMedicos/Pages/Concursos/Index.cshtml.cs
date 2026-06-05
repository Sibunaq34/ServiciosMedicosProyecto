using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Concursos
{
    // Persona C - Kenneth: Listado paginado, cambio de estado y eliminacion para OFE2.
    public class IndexModel : PageModel
    {
        private const int TamanoPagina = 10;
        private readonly IConcursoService _service;

        public IndexModel(
            IConcursoService service)
        {
            _service = service;
        }

        public IReadOnlyList<Concurso> Concursos { get; set; } =
            Array.Empty<Concurso>();

        public int Pagina { get; set; } = 1;

        public bool HaySiguientePagina =>
            Concursos.Count == TamanoPagina;

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
                Concursos =
                    await _service.ListarAsync(
                        Pagina,
                        TamanoPagina,
                        idUsuario.Value);
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
                Concursos = Array.Empty<Concurso>();
            }
            catch
            {
                MensajeError =
                    "Ocurrio un error al consultar los concursos.";

                Concursos = Array.Empty<Concurso>();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCambiarEstadoAsync(
            int idConcurso,
            string estado,
            int pagina = 1)
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            try
            {
                await _service.CambiarEstadoAsync(
                    idConcurso,
                    estado,
                    idUsuario.Value);

                MensajeExito =
                    "El estado del concurso fue actualizado correctamente.";
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
            }
            catch
            {
                MensajeError =
                    "Ocurrio un error al cambiar el estado del concurso.";
            }

            return RedirectToPage(
                "./Index",
                new { pagina });
        }

        public async Task<IActionResult> OnPostEliminarAsync(
            int idConcurso,
            int pagina = 1)
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            try
            {
                await _service.EliminarAsync(
                    idConcurso,
                    idUsuario.Value);

                MensajeExito =
                    "El concurso fue eliminado correctamente.";
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
            }
            catch
            {
                MensajeError =
                    "Ocurrio un error al eliminar el concurso.";
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
