using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;

namespace Servicios_Medicos.Pages.Empleados
{
    public class IndexModel : PageModel
    {
        private readonly IEmpleados _service;

        public IndexModel(IEmpleados service)
        {
            _service = service;
        }

        [BindProperty]
        public EmpleadoContratacion Empleado { get; set; }
            = new();

        [TempData]
        public string? Mensaje { get; set; }

        [TempData]
        public string? TipoMensaje { get; set; }

        public SelectList Oferentes { get; set; } = new SelectList(Enumerable.Empty<object>());

        public SelectList Puestos { get; set; } = new SelectList(Enumerable.Empty<object>());

        public SelectList Jefaturas { get; set; } = new SelectList(Enumerable.Empty<object>());

        public async Task OnGet()
        {
            await CargarCombos();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var exito = await _service.ContratarEmpleado(Empleado);

                TipoMensaje = exito ? "success" : "danger";
                Mensaje = exito
                    ? "Empleado contratado correctamente"
                    : "No fue posible contratar el empleado.";
            }
            catch (Exception ex)
            {
                TipoMensaje = "danger";
                Mensaje = ex.Message;
            }

            return RedirectToPage();
        }

        private async Task CargarCombos()
        {
            Oferentes = new SelectList(
                await _service.ListarOferentes(),
                "IdOferente",
                "NombreCompleto");

            Puestos = new SelectList(
                await _service.ListarPuestos(),
                "IdPuesto",
                "NombrePuesto");

            Jefaturas = new SelectList(
                await _service.ListarEmpleados(),
                "IdEmpleado",
                "NombreCompleto");
        }
    }
}
