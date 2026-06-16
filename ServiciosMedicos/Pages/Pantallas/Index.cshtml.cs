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

        [BindProperty(SupportsGet = true)]
        public int Pagina { get; set; } = 1;

        public int TotalPaginas { get; set; }

        private const int TamanoPagina = 10;

        public async Task OnGet()
        {
            var registros = (await _pantallas.Listar()).ToList();

            TotalPaginas = (int)Math.Ceiling(registros.Count / (double)TamanoPagina);

            if (TotalPaginas == 0)
                TotalPaginas = 1;

            if (Pagina < 1)
                Pagina = 1;

            if (Pagina > TotalPaginas)
                Pagina = TotalPaginas;

            ListaPantallas = registros
                .Skip((Pagina - 1) * TamanoPagina)
                .Take(TamanoPagina)
                .ToList();
        }

        public async Task<IActionResult> OnPostEliminar(int idPantalla)
        {
            try
            {
                await _pantallas.Eliminar(idPantalla);
                TempData["Mensaje"] = "Pantalla eliminada correctamente.";
            }
            catch
            {
                TempData["Error"] = "No se puede eliminar un registro con datos relacionados.";
            }

            return RedirectToPage();
        }
    }
}