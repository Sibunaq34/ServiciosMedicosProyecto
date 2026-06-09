using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Oferentes.PreparacionAcademica
{
    // Persona C - Kenneth: Listado paginado y eliminacion para OFE3.
    public class IndexModel : PageModel
    {
        private const int TamanoPagina = 10;
        private readonly IPreparacionAcademicaService _preparacionService;
        private readonly IOferenteService _oferenteService;

        public IndexModel(
            IPreparacionAcademicaService preparacionService,
            IOferenteService oferenteService)
        {
            _preparacionService = preparacionService;
            _oferenteService = oferenteService;
        }

        public OferenteDetalle? Oferente { get; set; }

        public IReadOnlyList<Servicios_Medicos.Entities.PreparacionAcademica> Preparaciones { get; set; } =
            Array.Empty<Servicios_Medicos.Entities.PreparacionAcademica>();

        public int IdOferente { get; set; }

        public int Pagina { get; set; } = 1;

        public bool HaySiguientePagina =>
            Preparaciones.Count == TamanoPagina;

        [TempData]
        public string? MensajeExito { get; set; }

        [TempData]
        public string? MensajeError { get; set; }

        public async Task<IActionResult> OnGetAsync(
            int idOferente,
            int pagina = 1)
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            IdOferente = idOferente;
            Pagina = pagina < 1 ? 1 : pagina;

            if (!await CargarOferenteAsync(
                    idUsuario.Value,
                    redirigirSiNoExiste: true))
            {
                return RedirectToPage(
                    "/Oferentes/Index");
            }

            try
            {
                Preparaciones =
                    await _preparacionService.ListarPorOferenteAsync(
                        IdOferente,
                        Pagina,
                        TamanoPagina,
                        idUsuario.Value);
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
                Preparaciones = Array.Empty<Servicios_Medicos.Entities.PreparacionAcademica>();
            }
            catch
            {
                MensajeError =
                    "Ocurrio un error al consultar la preparacion academica.";

                Preparaciones = Array.Empty<Servicios_Medicos.Entities.PreparacionAcademica>();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(
            int idPreparacion,
            int idOferente,
            int pagina = 1)
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            try
            {
                await _preparacionService.EliminarAsync(
                    idPreparacion,
                    idUsuario.Value);

                MensajeExito =
                    "La preparacion academica fue eliminada correctamente.";
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
            }
            catch
            {
                MensajeError =
                    "Ocurrio un error al eliminar la preparacion academica.";
            }

            return RedirectToPage(
                "./Index",
                new
                {
                    idOferente,
                    pagina
                });
        }

        private async Task<bool> CargarOferenteAsync(
            int idUsuario,
            bool redirigirSiNoExiste)
        {
            try
            {
                Oferente =
                    await _oferenteService.ObtenerAsync(
                        IdOferente,
                        idUsuario);
            }
            catch (InvalidOperationException ex)
            {
                TempData["MensajeError"] = ex.Message;
                return false;
            }
            catch
            {
                TempData["MensajeError"] =
                    "Ocurrio un error al consultar el oferente.";

                return false;
            }

            if (Oferente != null)
                return true;

            if (redirigirSiNoExiste)
            {
                TempData["MensajeError"] =
                    "El oferente no existe.";
            }

            return false;
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
