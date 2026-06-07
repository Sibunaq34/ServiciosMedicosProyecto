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

        public async Task OnGet()
        {
            ListaRoles =
                await _roles.Listar();
        }

        public async Task<IActionResult>
            OnPostEliminar(int idRol)
        {
            try
            {
                await _roles.Eliminar(idRol);

                TempData["Mensaje"] =
                    "Rol eliminado correctamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    ex.Message;
            }

            return RedirectToPage();
        }
    }
}