using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Servicios_Medicos.Services;
using System.ComponentModel.DataAnnotations;
using EntEntrevista = Servicios_Medicos.Entities.Entrevista;

namespace ServiciosMedicos.Pages.Entrevista
{
    public class AgendarModel : BasePageModel
    {
        protected override int? RolRequerido => 2;

        private readonly EntrevistaService _service;

        [BindProperty]
        [Required(ErrorMessage = "El oferente es requerido.")]
        public int IdOferente { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El empleado entrevistador es requerido.")]
        public int IdEmpleado { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "La fecha de entrevista es requerida.")]
        public DateTime FechaEntrevista { get; set; }

        public List<SelectListItem> OferentesItems { get; set; } = new();
        public List<SelectListItem> EmpleadosItems { get; set; } = new();
        public string? ErrorMensaje { get; set; }

        public AgendarModel(EntrevistaService service)
        {
            _service = service;
        }

        public async Task OnGetAsync()
        {
            await CargarDropdowns();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarDropdowns();
                return Page();
            }

            var entrevista = new EntEntrevista
            {
                IdOferente = IdOferente,
                IdEmpleado = IdEmpleado,
                FechaEntrevista = FechaEntrevista,
                Estado = "Pendiente"
            };

            var (exito, mensaje) = await _service.Agendar(entrevista, UsuarioId);

            if (!exito)
            {
                ErrorMensaje = mensaje;
                await CargarDropdowns();
                return Page();
            }

            TempData["TipoMensaje"] = "success";
            TempData["Mensaje"] = mensaje;
            return RedirectToPage("Index");
        }

        private async Task CargarDropdowns()
        {
            var oferentes = await _service.ObtenerOferentes();
            OferentesItems = oferentes.Select(o => new SelectListItem
            {
                Value = o.IdOferente.ToString(),
                Text = o.NombreCompleto
            }).ToList();

            var empleados = await _service.ObtenerEmpleados();
            EmpleadosItems = empleados.Select(e => new SelectListItem
            {
                Value = e.IdEmpleado.ToString(),
                Text = e.NombreCompleto
            }).ToList();
        }
    }
}
