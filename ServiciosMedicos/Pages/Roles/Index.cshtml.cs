using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Roles
{
    public class IndexModel : PageModel
    {
        private readonly IRoles _roles;

        public IndexModel(IRoles roles)
        {
            _roles = roles;
        }

        public IEnumerable<Rol> ListaRoles { get; set; }
            = new List<Rol>();

        [BindProperty(SupportsGet = true)]
        public int Pagina { get; set; } = 1;

        public int TotalPaginas { get; set; }

        private const int TamanoPagina = 10;

        public async Task OnGet()
        {
            var roles = (await _roles.Listar()).ToList();

            TotalPaginas = (int)Math.Ceiling(
                roles.Count / (double)TamanoPagina);

            if (TotalPaginas == 0)
                TotalPaginas = 1;

            if (Pagina < 1)
                Pagina = 1;

            if (Pagina > TotalPaginas)
                Pagina = TotalPaginas;

            ListaRoles = roles
                .Skip((Pagina - 1) * TamanoPagina)
                .Take(TamanoPagina)
                .ToList();
        }

        public async Task<IActionResult> OnPostEliminar(int id)
        {
            try
            {
                await _roles.Eliminar(id);

                TempData["Mensaje"] =
                    "Rol eliminado correctamente.";
            }
            catch
            {
                TempData["Error"] =
                    "No se puede eliminar un registro con datos relacionados.";
            }

            return RedirectToPage();
        }
    }
}