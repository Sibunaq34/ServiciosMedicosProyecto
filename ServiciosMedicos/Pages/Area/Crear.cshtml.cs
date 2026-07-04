using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Servicios_Medicos.Services;
using System.ComponentModel.DataAnnotations;
using EntArea = Servicios_Medicos.Entities.Area;
using ServiciosMedicos.Pages;
namespace ServiciosMedicos.Pages.Area
{
    public class CrearModel : BasePageModel
    {
        protected override int? RolRequerido => 2;

        private readonly IParametro _parametros;
        private readonly AreaService _service;

        [BindProperty]
        [Required(ErrorMessage = "El código de área es requerido.")]
        [MaxLength(20, ErrorMessage = "El código no puede superar 20 caracteres.")]
        public string CodigoArea { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "El nombre de área es requerido.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ ]+$", ErrorMessage = "Solo se permiten letras y espacios.")]
        public string NombreArea { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "El empleado es requerido.")]
        public int? IdEmpleado { get; set; }

        public List<SelectListItem> EmpleadosItems { get; set; } = new();
        public string? ErrorMensaje { get; set; }
        public int LongitudCodArea { get; set; }
        public int LongitudNombreArea { get; set; }

        public CrearModel(AreaService service, IParametro parametros)
        {
            _service = service;
            _parametros = parametros;
        }

        public async Task OnGetAsync()
        {

            await CargarEmpleados();
            LongitudCodArea = int.Parse(await _parametros.ObtenerValor("LONGITUD_COD_AREA"));
            LongitudNombreArea = int.Parse(await _parametros.ObtenerValor("LONGITUD_NOMBRE_AREA"));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarEmpleados();
                return Page();
            }

            var area = new EntArea
            {
                CodigoArea = CodigoArea.Trim().ToUpper(),
                NombreArea = NombreArea.Trim(),
                IdEmpleado = IdEmpleado!.Value
            };

            var (exito, mensaje) = await _service.Insertar(area, UsuarioId);

            if (!exito)
            {
                ErrorMensaje = mensaje;
                await CargarEmpleados();
                return Page();
            }

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
