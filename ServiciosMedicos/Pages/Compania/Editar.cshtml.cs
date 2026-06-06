using Microsoft.AspNetCore.Mvc;
using Servicios_Medicos.Services;
using System.ComponentModel.DataAnnotations;
using EntCompania = Servicios_Medicos.Entities.Compania;

namespace ServiciosMedicos.Pages.Compania
{
    public class EditarModel : BasePageModel
    {
        protected override int? RolRequerido => 1;

        private readonly CompaniaService _service;

        [BindProperty] public int IdCompania { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El código de compañía es requerido.")]
        [MaxLength(50, ErrorMessage = "El código no puede superar 50 caracteres.")]
        public string CodigoCompania { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "El nombre es requerido.")]
        [MaxLength(150, ErrorMessage = "El nombre no puede superar 150 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        public string? ErrorMensaje { get; set; }

        public EditarModel(CompaniaService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var compania = await _service.ObtenerPorId(id);
            if (compania == null)
                return RedirectToPage("Index");

            IdCompania = compania.IdCompania;
            CodigoCompania = compania.CodigoCompania;
            Nombre = compania.Nombre;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var antes = await _service.ObtenerPorId(IdCompania);
            if (antes == null)
                return RedirectToPage("Index");

            var despues = new EntCompania
            {
                IdCompania = IdCompania,
                CodigoCompania = CodigoCompania.Trim(),
                Nombre = Nombre.Trim()
            };

            var (exito, mensaje) = await _service.Actualizar(antes, despues, UsuarioId);

            if (!exito)
            {
                ErrorMensaje = mensaje;
                return Page();
            }

            TempData["TipoMensaje"] = "success";
            TempData["Mensaje"] = mensaje;
            return RedirectToPage("Index");
        }
    }
}
