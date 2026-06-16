using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;

namespace ServiciosMedicos.Pages.Gerente
{
    public class CrearParametroModel : PageModel
    {
        private readonly IParametro _parametro;
        public CrearParametroModel(IParametro parametro)
        {
            _parametro = parametro;
        }

        [BindProperty]
        public Parametros Parametro { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {

                int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

                if (!idUsuario.HasValue)
                {
                    return RedirectToPage("/Login");
                }
                await _parametro.Insertar(Parametro, idUsuario.Value);

                TempData["Mensaje"] =
                    $"Se registro correctamente";


            return RedirectToPage();
        }
    }
}
