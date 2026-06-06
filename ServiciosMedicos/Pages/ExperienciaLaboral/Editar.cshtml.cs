using Microsoft.AspNetCore.Mvc;
using Servicios_Medicos.Services;
using System.ComponentModel.DataAnnotations;
using EntExperiencia = Servicios_Medicos.Entities.ExperienciaLaboral;

namespace ServiciosMedicos.Pages.ExperienciaLaboral
{
    public class EditarModel : BasePageModel
    {
        protected override int? RolRequerido => 2;

        private readonly ExperienciaLaboralService _service;

        [BindProperty] public int IdExperiencia { get; set; }
        [BindProperty] public int OfertanteId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El nombre de la empresa es requerido.")]
        [MaxLength(100, ErrorMessage = "El nombre de la empresa no puede superar 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ0-9 ]+$", ErrorMessage = "Solo se permiten letras, números y espacios.")]
        public string NombreEmpresa { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "El puesto desempeñado es requerido.")]
        [MaxLength(100, ErrorMessage = "El puesto no puede superar 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ0-9 ]+$", ErrorMessage = "Solo se permiten letras, números y espacios.")]
        public string PuestoDesempenado { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "La fecha de inicio es requerida.")]
        public DateTime FechaInicio { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "La fecha de fin es requerida.")]
        public DateTime FechaFin { get; set; }

        public string? ErrorMensaje { get; set; }

        public EditarModel(ExperienciaLaboralService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var exp = await _service.ObtenerPorId(id);
            if (exp == null)
                return RedirectToPage("Index", new { ofertanteId = 0 });

            IdExperiencia = exp.IdExperiencia;
            OfertanteId = exp.IdOferente;
            NombreEmpresa = exp.NombreEmpresa;
            PuestoDesempenado = exp.PuestoDesempenado;
            FechaInicio = exp.FechaInicio;
            FechaFin = exp.FechaFin;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (FechaFin < FechaInicio)
            {
                ErrorMensaje = "La fecha de fin no puede ser menor a la fecha de inicio.";
                return Page();
            }

            var antes = await _service.ObtenerPorId(IdExperiencia);
            if (antes == null)
                return RedirectToPage("Index", new { ofertanteId = OfertanteId });

            var despues = new EntExperiencia
            {
                IdExperiencia = IdExperiencia,
                IdOferente = OfertanteId,
                NombreEmpresa = NombreEmpresa.Trim(),
                PuestoDesempenado = PuestoDesempenado.Trim(),
                FechaInicio = FechaInicio,
                FechaFin = FechaFin
            };

            var (exito, mensaje) = await _service.Actualizar(antes, despues, UsuarioId);

            if (!exito)
            {
                ErrorMensaje = mensaje;
                return Page();
            }

            TempData["TipoMensaje"] = "success";
            TempData["Mensaje"] = mensaje;
            return RedirectToPage("Index", new { ofertanteId = OfertanteId });
        }
    }
}
