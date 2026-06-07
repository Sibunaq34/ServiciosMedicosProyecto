using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Servicios_Medicos.Services;
using System.ComponentModel.DataAnnotations;
using EntEntrevista = Servicios_Medicos.Entities.Entrevista;

namespace ServiciosMedicos.Pages.Entrevista
{
    public class EditarModel : BasePageModel
    {
        protected override int? RolRequerido => 2;

        private readonly EntrevistaService _service;

        [BindProperty] public int IdEntrevista { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El empleado entrevistador es requerido.")]
        public int IdEmpleado { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "La fecha de entrevista es requerida.")]
        public DateTime FechaEntrevista { get; set; }

        public string NombreOferente { get; set; } = string.Empty;
        public List<SelectListItem> EmpleadosItems { get; set; } = new();
        public string? ErrorMensaje { get; set; }

        public EditarModel(EntrevistaService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var entrevista = await _service.ObtenerPorId(id);
            if (entrevista == null)
                return RedirectToPage("Index");

            if (entrevista.Estado == "Realizada")
            {
                TempData["TipoMensaje"] = "danger";
                TempData["Mensaje"] = "No se puede modificar una entrevista que ya fue realizada.";
                return RedirectToPage("Index");
            }

            IdEntrevista = entrevista.IdEntrevista;
            IdEmpleado = entrevista.IdEmpleado;
            FechaEntrevista = entrevista.FechaEntrevista;
            NombreOferente = entrevista.NombreOferente;

            await CargarEmpleados();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarEmpleados();
                return Page();
            }

            var antes = await _service.ObtenerPorId(IdEntrevista);
            if (antes == null)
                return RedirectToPage("Index");

            if (antes.Estado == "Realizada")
            {
                TempData["TipoMensaje"] = "danger";
                TempData["Mensaje"] = "No se puede modificar una entrevista que ya fue realizada.";
                return RedirectToPage("Index");
            }

            var despues = new EntEntrevista
            {
                IdEntrevista = IdEntrevista,
                IdOferente = antes.IdOferente,
                IdEmpleado = IdEmpleado,
                FechaEntrevista = FechaEntrevista,
                Estado = antes.Estado
            };

            var (exito, mensaje) = await _service.Actualizar(antes, despues, UsuarioId);

            if (!exito)
            {
                ErrorMensaje = mensaje;
                NombreOferente = antes.NombreOferente;
                await CargarEmpleados();
                return Page();
            }

            TempData["TipoMensaje"] = "success";
            TempData["Mensaje"] = mensaje;
            return RedirectToPage("Index");
        }

        private async Task CargarEmpleados()
        {
            var empleados = await _service.ObtenerEmpleados();
            EmpleadosItems = empleados.Select(e => new SelectListItem
            {
                Value = e.IdEmpleado.ToString(),
                Text = e.NombreCompleto
            }).ToList();
        }
    }
}
