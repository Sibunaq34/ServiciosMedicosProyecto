using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;

namespace Servicios_Medicos.Pages.Puestos
{
    public class CreateModel : PageModel
    {
        private readonly IPuestos _puestosService;

        public CreateModel(IPuestos puestosService)
        {
            _puestosService = puestosService;
        }

        [BindProperty]
        public Puesto Puesto { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var resultado = await _puestosService.InsertarPuesto(Puesto);

            if (!resultado)
            {
                ModelState.AddModelError("", "No se pudo guardar el puesto");
                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
