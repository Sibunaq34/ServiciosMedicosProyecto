using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Pantallas
{
    public class CrearModel : PageModel
    {
        private readonly IPantallas _pantallas;

        public CrearModel(IPantallas pantallas)
        {
            _pantallas = pantallas;
        }

        [BindProperty]
        public Pantalla Pantalla { get; set; }
            = new();

        public void OnGet()
        {
            Pantalla.Activo = true;
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                await _pantallas.Crear(Pantalla);

                TempData["Mensaje"] =
                    "Pantalla creada correctamente";

                return RedirectToPage("Index");
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