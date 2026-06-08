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

        public SelectList Oferentes { get; set; }

        public SelectList Puestos { get; set; }

        public SelectList Jefaturas { get; set; }

        public async Task OnGet()
        {
            await CargarCombos();
        }

        public async Task<IActionResult> OnPost()
        {
            await _service.ContratarEmpleado(Empleado);

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
