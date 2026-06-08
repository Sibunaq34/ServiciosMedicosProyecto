using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Roles
{
    public class AsignarPantallasModel : PageModel
    {
        private readonly IRoles _roles;

        public AsignarPantallasModel(IRoles roles)
        {
            _roles = roles;
        }

        [BindProperty]
        public int IdRol { get; set; }

        public IEnumerable<Pantalla> ListaPantallas { get; set; }
            = new List<Pantalla>();

        [BindProperty]
        public List<int> PantallasSeleccionadas { get; set; }
            = new();

        public async Task<IActionResult> OnGet(int idRol)
        {
            IdRol = idRol;

            ListaPantallas =
                await _roles.ListarPantallasPorRol(idRol);

            PantallasSeleccionadas =
                ListaPantallas
                    .Where(p => p.Activo)
                    .Select(p => p.IdPantalla)
                    .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            await _roles.GuardarPantallasRol(
                IdRol,
                PantallasSeleccionadas);

            TempData["Mensaje"] =
                "Pantallas asignadas correctamente.";

            return RedirectToPage("Index");
        }
    }
}