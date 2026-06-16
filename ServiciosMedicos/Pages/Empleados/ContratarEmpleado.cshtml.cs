using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Empleados
{
    public class ContratarEmpleadoModel : PageModel
    {
        private readonly IEmpleados _service;
        private readonly ILogger<ContratarEmpleadoModel> _logger;
        private readonly IWebHostEnvironment _environment;

        public ContratarEmpleadoModel(
            IEmpleados service,
            ILogger<ContratarEmpleadoModel> logger,
            IWebHostEnvironment environment)
        {
            _service = service;
            _logger = logger;
            _environment = environment;
        }

        [BindProperty]
        public EmpleadoContratacion Empleado { get; set; } = new();

        public SelectList Oferentes { get; set; } = new SelectList(Enumerable.Empty<object>());
        public SelectList Puestos { get; set; } = new SelectList(Enumerable.Empty<object>());
        public SelectList Jefaturas { get; set; } = new SelectList(Enumerable.Empty<object>());

        [TempData]
        public string? Mensaje { get; set; }

        [TempData]
        public string? TipoMensaje { get; set; }

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

            try
            {
                var resultado = await _service.ContratarEmpleado(Empleado);
                if (!resultado)
                {
                    ModelState.AddModelError("", "No se pudo contratar el empleado");
                    await CargarCombos();
                    return Page();
                }

                TipoMensaje = "success";
                Mensaje = "Empleado contratado correctamente.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al contratar empleado. IdOferente: {IdOferente}, IdPuesto: {IdPuesto}, IdJefatura: {IdJefatura}",
                    Empleado.IdOferente,
                    Empleado.IdPuesto,
                    Empleado.IdJefatura);

                var mensaje = _environment.IsDevelopment()
                    ? $"Error técnico: {ex.Message}"
                    : "No se pudo contratar el empleado";

                ModelState.AddModelError("", mensaje);
                await CargarCombos();
                return Page();
            }
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
