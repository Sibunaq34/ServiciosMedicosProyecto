using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;
using ServiciosMedicos.Pages;

namespace Servicios_Medicos.Pages.Puestos
{
    public class CreateModel : BasePageModel
    {
        private readonly IPuestos _puestosService;

        public CreateModel(IPuestos puestosService)
        {
            _puestosService = puestosService;
        }

        [BindProperty]
        public Puesto Puesto { get; set; } = new();

        [TempData]
        public string? Mensaje { get; set; }

        [TempData]
        public string? TipoMensaje { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var resultado = await _puestosService.InsertarPuesto(Puesto);

                if (!resultado)
                {
                    ModelState.AddModelError("", "No se pudo guardar el puesto");
                    return Page();
                }

                TipoMensaje = "success";
                Mensaje = "Puesto creado correctamente.";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }
        }
    }
}
