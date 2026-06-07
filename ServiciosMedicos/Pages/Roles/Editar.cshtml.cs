using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Roles
{
    public class EditarModel : PageModel
    {
        private readonly IRoles _roles;

        public EditarModel(IRoles roles)
        {
            _roles = roles;
        }

        [BindProperty]
        public Rol Rol { get; set; }
            = new();

        public async Task<IActionResult>
            OnGet(int idRol)
        {
            var rol =
                await _roles.ObtenerPorId(idRol);

            if (rol == null)
            {
                return RedirectToPage("Index");
            }

            Rol = rol;

            return Page();
        }

        public async Task<IActionResult>
            OnPost()
        {
            try
            {
                await _roles.Actualizar(Rol);

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