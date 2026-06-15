using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Pantallas
{
    public class EditarModel : PageModel
    {
        private readonly IPantallas _pantallas;

        public EditarModel(IPantallas pantallas)
        {
            _pantallas = pantallas;
        }

        [BindProperty]
        public Pantalla Pantalla { get; set; }
            = new();

        public async Task<IActionResult>
            OnGet(int idPantalla)
        {
            var pantalla =
                await _pantallas.ObtenerPorId(idPantalla);

            if (pantalla == null)
            {
                return RedirectToPage("Index");
            }

            Pantalla = pantalla;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrWhiteSpace(Pantalla.NombrePantalla))
            {
                TempData["Validacion"] = "Debe completar todos los campos requeridos.";
                return Page();
            }

            if (Pantalla.NombrePantalla.Length > 100)
            {
                TempData["Validacion"] = "El nombre de la pantalla no puede superar los 100 caracteres.";
                return Page();
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(Pantalla.NombrePantalla, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                TempData["Validacion"] = "El nombre de la pantalla solo puede contener letras y espacios.";
                return Page();
            }

            try
            {
                await _pantallas.Actualizar(Pantalla);
                TempData["Mensaje"] = "Pantalla actualizada correctamente.";
                return RedirectToPage("Index");
            }
            catch
            {
                TempData["Error"] = "No fue posible actualizar la pantalla.";
                return Page();
            }
        }
    }
}