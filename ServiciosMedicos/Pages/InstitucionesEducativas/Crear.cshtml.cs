using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.InstitucionesEducativas
{
    // Persona C - Kenneth: Creacion de instituciones educativas GEN5.
    public class CrearModel : PageModel
    {
        private readonly IInstitucionEducativaService _service;

        public CrearModel(
            IInstitucionEducativaService service)
        {
            _service = service;
        }

        [BindProperty]
        public InstitucionEducativa Institucion { get; set; } =
            new();

        public string? MensajeError { get; set; }

        public IActionResult OnGet()
        {
            if (ObtenerIdUsuario() == null)
                return RedirigirALogin();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _service.CrearAsync(
                    Institucion,
                    idUsuario.Value);

                TempData["MensajeExito"] =
                    "La institución educativa fue registrada correctamente.";

                return RedirectToPage("./Index");
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
            }
            catch
            {
                MensajeError =
                    "Ocurrió un error al registrar la institución educativa.";
            }

            return Page();
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
