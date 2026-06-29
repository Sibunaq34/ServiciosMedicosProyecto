using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;
using ServiciosMedicos.Pages;
namespace ServiciosMedicos.Pages.InstitucionesEducativas
{
    // Persona C - Kenneth: Edicion con datos precargados para GEN5.
    public class EditarModel : BasePageModel
    {
        private readonly IInstitucionEducativaService _service;

        public EditarModel(
            IInstitucionEducativaService service)
        {
            _service = service;
        }

        [BindProperty]
        public InstitucionEducativa Institucion { get; set; } =
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
                var institucion =
                    await _service.ObtenerAsync(
                        id,
                        idUsuario.Value);

                if (institucion == null)
                {
                    TempData["MensajeError"] =
                        "La institución educativa no existe.";

                    return RedirectToPage("./Index");
                }

                Institucion = institucion;
            }
            catch (InvalidOperationException ex)
            {
                TempData["MensajeError"] = ex.Message;

                return RedirectToPage("./Index");
            }
            catch
            {
                TempData["MensajeError"] =
                    "Ocurrió un error al consultar la institución educativa.";

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

            Institucion.IdInstitucion = id;

            try
            {
                await _service.ActualizarAsync(
                    Institucion,
                    idUsuario.Value);

                TempData["MensajeExito"] =
                    "La institución educativa fue actualizada correctamente.";

                return RedirectToPage("./Index");
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
            }
            catch
            {
                MensajeError =
                    "Ocurrió un error al actualizar la institución educativa.";
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
