using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Servicios_Medicos.Entities;
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

        public SelectList Empleados { get; set; }

        public async Task OnGet()
        {
            Empleados =
                new SelectList(
                    await _empleadosService
                        .ListarEmpleados(),
                    "IdEmpleado",
                    "NombreCompleto");
        }

        public async Task<IActionResult> OnPost()
        {
            await _accionesService
                .InsertarAccion(Accion);

            return RedirectToPage("Index");
        }
    }
}
