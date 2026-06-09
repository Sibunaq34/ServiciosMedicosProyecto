using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Roles
{
    public class CrearModel : PageModel
    {
        private readonly IRoles _roles;       
        private readonly IParametro _parametros;
        public CrearModel(IRoles roles, IParametro parametros)
        {
            _roles = roles;
            _parametros = parametros;
        }

        [BindProperty]
        public Rol Rol { get; set; } = new();
        public int LongitudNombreRol { get; set; }
        public async Task OnGet()
        {
            LongitudNombreRol = int.Parse(await _parametros.ObtenerValor("LONGITUD_NOMBRE_ROL"));
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
                LongitudNombreRol = int.Parse(await _parametros.ObtenerValor("LONGITUD_NOMBRE_ROL"));
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return Page();
            }
        }
    }
}