using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Oferentes.PreparacionAcademica
{
    // Persona C - Kenneth: Edicion con datos precargados para OFE3.
    public class EditarModel : PageModel
    {
        private readonly IPreparacionAcademicaService _preparacionService;
        private readonly IOferenteService _oferenteService;
        private readonly IInstitucionEducativaService _institucionService;

        public EditarModel(
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
            int id)
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            try
            {
                var preparacion =
                    await _preparacionService.ObtenerAsync(
                        id,
                        idUsuario.Value);

                if (preparacion == null)
                {
                    TempData["MensajeError"] =
                        "La preparacion academica no existe.";

                    return RedirectToPage(
                        "/Oferentes/Index");
                }

                Preparacion = preparacion;
            }
            catch (InvalidOperationException ex)
            {
                TempData["MensajeError"] = ex.Message;

                return RedirectToPage(
                    "/Oferentes/Index");
            }
            catch
            {
                TempData["MensajeError"] =
                    "Ocurrio un error al consultar la preparacion academica.";

                return RedirectToPage(
                    "/Oferentes/Index");
            }

            await CargarDatosAsync(
                idUsuario.Value,
                Preparacion.IdOferente);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(
            int id)
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            Preparacion.IdPreparacion = id;

            ModelState.Clear();
            TryValidateModel(Preparacion, nameof(Preparacion));

            await CargarDatosAsync(
                idUsuario.Value,
                Preparacion.IdOferente);

            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _preparacionService.ActualizarAsync(
                    Preparacion,
                    idUsuario.Value);

                TempData["MensajeExito"] =
                    "La preparacion academica fue actualizada correctamente.";

                return RedirectToPage(
                    "./Index",
                    new { idOferente = Preparacion.IdOferente });
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
            }
            catch
            {
                MensajeError =
                    "Ocurrio un error al actualizar la preparacion academica.";
            }

            return Page();
        }

        private async Task CargarDatosAsync(
            int idUsuario,
            int idOferente)
        {
            try
            {
                Oferente =
                    await _oferenteService.ObtenerAsync(
                        idOferente,
                        idUsuario);

                Instituciones =
                    await _institucionService.ListarAsync(
                        1,
                        100,
                        idUsuario);
            }
            catch
            {
                Instituciones = Array.Empty<InstitucionEducativa>();
                MensajeError ??=
                    "Ocurrio un error al cargar los datos de preparacion academica.";
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
