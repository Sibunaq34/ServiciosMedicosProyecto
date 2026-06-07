using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Roles
{
    public class CrearModel : PageModel
    {
        private readonly IRoles _roles;

        public CrearModel(IRoles roles)
        {
            _roles = roles;
        }

        [BindProperty]
        public Rol Rol { get; set; }
            = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                await _roles.Crear(Rol);

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