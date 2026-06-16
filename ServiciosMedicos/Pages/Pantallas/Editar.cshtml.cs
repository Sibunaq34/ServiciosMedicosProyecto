using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Pantallas
{
    public class EditarModel : PageModel
    {
        private readonly IPantallas _pantallas;

        public EditarModel(IPantallas pantallas)
        {
            _pantallas = pantallas;
        }

        [BindProperty]
        public Pantalla Pantalla { get; set; }
            = new();

        public async Task<IActionResult>
            OnGet(int idPantalla)
        {
            var pantalla =
                await _pantallas.ObtenerPorId(idPantalla);

            if (pantalla == null)
            {
                return RedirectToPage("Index");
            }

            Pantalla = pantalla;

            return Page();
        }

        public async Task<IActionResult>
            OnPost()
        {
            try
            {
                await _pantallas.Actualizar(Pantalla);

                TempData["Mensaje"] =
                    "Pantalla actualizada correctamente";

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