using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Roles
{
    public class EditarModel : PageModel
    {
        private readonly IRoles _roles;
        private readonly IParametro _parametros;

        public EditarModel(IRoles roles, IParametro parametros)
        {
            _roles = roles;
            _parametros = parametros;
        }

        [BindProperty]
        public Rol Rol { get; set; } = new();
        public int LongitudNombreRol { get; set; }

        public async Task<IActionResult> OnGet(int idRol)
        {
            var rol =
                await _roles.ObtenerPorId(idRol);

            if (rol == null)
            {
                return RedirectToPage("Index");
            }

            Rol = rol;
            LongitudNombreRol = int.Parse(await _parametros.ObtenerValor("LONGITUD_NOMBRE_ROL"));

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
                LongitudNombreRol = int.Parse(await _parametros.ObtenerValor("LONGITUD_NOMBRE_ROL"));
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return Page();
            }
        }
    }
}