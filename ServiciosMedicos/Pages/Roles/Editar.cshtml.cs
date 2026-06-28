using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;
using ServiciosMedicos.Pages;
namespace ServiciosMedicos.Pages.Roles
{
    public class EditarModel : BasePageModel
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

                await _roles.Actualizar(Rol);

                TempData["Mensaje"] =
                    "Rol actualizado correctamente.";

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
                        "No fue posible actualizar el rol.";
                }

                return Page();
            }
        }
    }
}