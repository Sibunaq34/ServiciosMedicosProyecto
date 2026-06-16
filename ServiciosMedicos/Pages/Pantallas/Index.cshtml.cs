using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Pantallas
{
    public class IndexModel : PageModel
    {
        private readonly IPantallas _pantallas;

        public IndexModel(IPantallas pantallas)
        {
            _pantallas = pantallas;
        }

        public IEnumerable<Pantalla> ListaPantallas { get; set; }
            = new List<Pantalla>();

        public async Task OnGet()
        {
            ListaPantallas = await _pantallas.Listar();
        }

        public async Task<IActionResult> OnPostEliminar(int idPantalla)
        {
            try
            {
                await _pantallas.Eliminar(idPantalla);

                TempData["Mensaje"] =
                    "Pantalla eliminada correctamente.";
            }
            catch
            {
                TempData["Error"] =
                    "No se puede eliminar una pantalla que tiene roles asociados.";
            }

            return RedirectToPage();
        }
    }
}