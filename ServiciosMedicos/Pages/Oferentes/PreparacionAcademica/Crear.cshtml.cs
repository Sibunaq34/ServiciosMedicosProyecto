using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Oferentes.PreparacionAcademica
{
    // Persona C - Kenneth: Creacion de preparacion academica OFE3.
    public class CrearModel : PageModel
    {
        private readonly IPreparacionAcademicaService _preparacionService;
        private readonly IOferenteService _oferenteService;
        private readonly IInstitucionEducativaService _institucionService;

        public CrearModel(
            IPreparacionAcademicaService preparacionService,
            IOferenteService oferenteService,
            IInstitucionEducativaService institucionService)
        {
            _preparacionService = preparacionService;
            _oferenteService = oferenteService;
            _institucionService = institucionService;
        }

        [BindProperty]
        public Servicios_Medicos.Entities.PreparacionAcademica Preparacion { get; set; } =
            new();

        public OferenteDetalle? Oferente { get; set; }

        public IReadOnlyList<InstitucionEducativa> Instituciones { get; set; } =
            Array.Empty<InstitucionEducativa>();

        public string? MensajeError { get; set; }

        public async Task<IActionResult> OnGetAsync(
            int idOferente)
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            Preparacion.IdOferente = idOferente;

            if (!await CargarDatosAsync(
                    idUsuario.Value,
                    idOferente))
            {
                return RedirectToPage(
                    "/Oferentes/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(
            int idOferente)
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            Preparacion.IdOferente = idOferente;

            ModelState.Clear();
            TryValidateModel(Preparacion, nameof(Preparacion));

            if (!await CargarDatosAsync(
                    idUsuario.Value,
                    idOferente))
            {
                return RedirectToPage(
                    "/Oferentes/Index");
            }

            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _preparacionService.CrearAsync(
                    Preparacion,
                    idUsuario.Value);

                TempData["MensajeExito"] =
                    "La preparacion academica fue registrada correctamente.";

                return RedirectToPage(
                    "./Index",
                    new { idOferente });
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
            }
            catch
            {
                MensajeError =
                    "Ocurrio un error al registrar la preparacion academica.";
            }

            return Page();
        }

        private async Task<bool> CargarDatosAsync(
            int idUsuario,
            int idOferente)
        {
            try
            {
                Oferente =
                    await _oferenteService.ObtenerAsync(
                        idOferente,
                        idUsuario);

                if (Oferente == null)
                {
                    TempData["MensajeError"] =
                        "El oferente no existe.";

                    return false;
                }

                Instituciones =
                    await _institucionService.ListarAsync(
                        1,
                        100,
                        idUsuario);

                return true;
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
                return true;
            }
            catch
            {
                MensajeError =
                    "Ocurrio un error al cargar los datos de preparacion academica.";

                return true;
            }
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
