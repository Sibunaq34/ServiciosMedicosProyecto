using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;
using ServiciosMedicos.Pages;

namespace ServiciosMedicos.Pages.Pantallas
{
    public class CrearModel : BasePageModel
    {
        private readonly IPantallas _pantallas;

        public CrearModel(IPantallas pantallas)
        {
            _pantallas = pantallas;
        }

        [BindProperty]
        public Pantalla Pantalla { get; set; }
            = new();

        public void OnGet()
        {
            Pantalla.Activo = true;
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
                await _pantallas.Crear(Pantalla);
                TempData["Mensaje"] = "Pantalla creada correctamente.";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Duplicate") || ex.Message.Contains("duplicado") || ex.Message.Contains("Duplicate entry"))
                    TempData["Validacion"] = "Ya existe una pantalla con ese nombre.";
                else
                    TempData["Error"] = "Ha ocurrido un error inesperado. Intente nuevamente.";

                return Page();
            }
        }
    }
}