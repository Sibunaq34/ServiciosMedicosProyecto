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
                LongitudNombreRol =
                    int.Parse(await _parametros.ObtenerValor("LONGITUD_NOMBRE_ROL"));

                if (string.IsNullOrWhiteSpace(Rol.NombreRol))
                {
                    TempData["Validacion"] =
                        "Debe completar todos los campos requeridos.";

                    return Page();
                }

                if (Rol.NombreRol.Length > LongitudNombreRol)
                {
                    TempData["Validacion"] =
                        "El nombre del rol no puede superar los 40 caracteres.";

                    return Page();
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(
                        Rol.NombreRol,
                        @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                {
                    TempData["Validacion"] =
                        "El nombre del rol solo puede contener letras y espacios.";

                    return Page();
                }

                await _roles.Crear(Rol);

                TempData["Mensaje"] =
                    "Rol creado correctamente.";

                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                LongitudNombreRol =
                    int.Parse(await _parametros.ObtenerValor("LONGITUD_NOMBRE_ROL"));

                if (ex.Message.Contains("existe") ||
                    ex.Message.Contains("duplicado") ||
                    ex.Message.Contains("Duplicate"))
                {
                    TempData["Error"] =
                        "Ya existe un rol con ese nombre.";
                }
                else
                {
                    TempData["Error"] =
                        "Ha ocurrido un error inesperado. Intente nuevamente.";
                }

                return Page();
            }
        }
    }
}