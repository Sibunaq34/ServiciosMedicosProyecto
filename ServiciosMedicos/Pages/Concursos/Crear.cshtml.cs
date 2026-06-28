using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;
using ServiciosMedicos.Pages;
namespace ServiciosMedicos.Pages.Concursos
{
    // Persona C - Kenneth: Creacion de concursos OFE2.
    public class CrearModel : BasePageModel
    {
        private readonly IConcursoService _service;

        public CrearModel(
            IConcursoService service)
        {
            _service = service;
        }

        [BindProperty]
        public Concurso Concurso { get; set; } =
            new();

        public string? MensajeError { get; set; }

        public IActionResult OnGet()
        {
            if (ObtenerIdUsuario() == null)
                return RedirigirALogin();

            Concurso.Estado = "Vigente";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            Concurso.Estado = "Vigente";

            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _service.CrearAsync(
                    Concurso,
                    idUsuario.Value);

                TempData["MensajeExito"] =
                    "El concurso fue registrado correctamente.";

                return RedirectToPage("./Index");
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
            }
            catch
            {
                MensajeError =
                    "Ocurrio un error al registrar el concurso.";
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
