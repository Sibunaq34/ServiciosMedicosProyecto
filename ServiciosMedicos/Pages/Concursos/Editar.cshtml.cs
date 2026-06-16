using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;
using ServiciosMedicos.Pages;
namespace ServiciosMedicos.Pages.Concursos
{
    // Persona C - Kenneth: Edicion con datos precargados para OFE2.
    public class EditarModel : BasePageModel
    {
        private readonly IConcursoService _service;

        public EditarModel(
            IConcursoService service)
        {
            _service = service;
        }

        [BindProperty]
        public Concurso Concurso { get; set; } =
            new();

        public string? MensajeError { get; set; }

        public async Task<IActionResult> OnGetAsync(
            int id)
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            try
            {
                var concurso =
                    await _service.ObtenerAsync(
                        id,
                        idUsuario.Value);

                if (concurso == null)
                {
                    TempData["MensajeError"] =
                        "El concurso no existe.";

                    return RedirectToPage("./Index");
                }

                Concurso = concurso;
            }
            catch (InvalidOperationException ex)
            {
                TempData["MensajeError"] = ex.Message;

                return RedirectToPage("./Index");
            }
            catch
            {
                TempData["MensajeError"] =
                    "Ocurrio un error al consultar el concurso.";

                return RedirectToPage("./Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(
            int id)
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            Concurso.IdConcurso = id;

            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _service.ActualizarAsync(
                    Concurso,
                    idUsuario.Value);

                TempData["MensajeExito"] =
                    "El concurso fue actualizado correctamente.";

                return RedirectToPage("./Index");
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
            }
            catch
            {
                MensajeError =
                    "Ocurrio un error al actualizar el concurso.";
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
