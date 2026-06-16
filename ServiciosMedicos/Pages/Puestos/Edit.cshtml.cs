using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;

namespace Servicios_Medicos.Pages.Puestos
{
    public class EditModel : PageModel
    {
        private readonly IPuestos _puestosService;

        public EditModel(IPuestos puestosService)
        {
            _puestosService = puestosService;
        }

        [BindProperty]
        public Puesto Puesto { get; set; } = new();

        public async Task<IActionResult> OnGet(int id)
        {
            var puesto = await _puestosService.ObtenerPuesto(id);

            if (puesto == null)
                return RedirectToPage("Index");

            Puesto = puesto;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var resultado =
                await _puestosService.ActualizarPuesto(Puesto);

            if (!resultado)
            {
                ModelState.AddModelError("", "Error al actualizar");
                return Page();
            }

            TempData["TipoMensaje"] = "success";
            TempData["Mensaje"] = "Puesto actualizado correctamente.";
            return RedirectToPage("Index");
        }
    }
}
