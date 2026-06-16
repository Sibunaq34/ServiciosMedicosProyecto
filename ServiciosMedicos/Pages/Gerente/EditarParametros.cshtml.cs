using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;

namespace ServiciosMedicos.Pages.Gerente
{
    public class EditarParametrosModel : PageModel
    {

        private readonly IParametro _parametro;

        public EditarParametrosModel(IParametro parametro)
        {
            _parametro = parametro;
        }

        [BindProperty]

        public Parametros Parametro { get; set; } = new();

        public async Task<IActionResult> OnGet(int idParametro)
        {

            var parame = await _parametro.ObtenerPorId(idParametro);

            if (parame == null)
            {
                return RedirectToPage("Parametros");
            }

            Parametro = parame;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            try
            {

                await _parametro.Actualizar(Parametro, idUsuario.Value);
                return RedirectToPage("Parametros");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);
                return Page();
            }
        }
    }
}
