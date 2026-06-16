using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;

namespace Servicios_Medicos.Pages.AccionesPersonal
{
    public class CreateModel : PageModel
    {
        private readonly IAccionesPersonal _accionesService;
        private readonly IEmpleados _empleadosService;

        public CreateModel(
            IAccionesPersonal accionesService,
            IEmpleados empleadosService)
        {
            _accionesService = accionesService;
            _empleadosService = empleadosService;
        }

        [BindProperty]
        public AccionPersonal Accion { get; set; }
            = new();

        public SelectList Empleados { get; set; } =
            new SelectList(Enumerable.Empty<object>());

        public async Task OnGet()
        {
            await CargarCombos();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos();
                return Page();
            }

            var resultado = await _accionesService
                .InsertarAccion(Accion);

            if (!resultado)
            {
                ModelState.AddModelError("", "No se pudo guardar la acción de personal");
                await CargarCombos();
                return Page();
            }

            TempData["TipoMensaje"] = "success";
            TempData["Mensaje"] = "Acción de personal registrada correctamente.";
            return RedirectToPage("Index");
        }

        private async Task CargarCombos()
        {
            Empleados =
                new SelectList(
                    await _empleadosService
                        .ListarEmpleados(),
                    "IdEmpleado",
                    "NombreCompleto");
        }
    }
}
