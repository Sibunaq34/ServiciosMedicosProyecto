using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;
using ServiciosMedicos.Pages;

namespace Servicios_Medicos.Pages.AccionesPersonal
{
    public class CreateModel : BasePageModel
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

        [TempData]
        public string? Mensaje { get; set; }

        [TempData]
        public string? TipoMensaje { get; set; }

        public SelectList Empleados { get; set; } =
            new SelectList(Enumerable.Empty<object>());

        public async Task OnGet()
        {
            Accion.FechaAccion = DateTime.Today;
            await CargarEmpleados();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                await CargarEmpleados();
                return Page();
            }

            try
            {
                var resultado =
                    await _accionesService
                        .InsertarAccion(Accion);

                if (!resultado)
                {
                    ModelState.AddModelError("", "No se pudo guardar la acción de personal");
                    await CargarEmpleados();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await CargarEmpleados();
                return Page();
            }

            TipoMensaje = "success";
            Mensaje = "Acción de personal creada correctamente.";
            return RedirectToPage("Index");
        }

        private async Task CargarEmpleados()
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
