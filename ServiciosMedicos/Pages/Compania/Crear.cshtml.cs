using Microsoft.AspNetCore.Mvc;
using Servicios_Medicos.Services;
using System.ComponentModel.DataAnnotations;
using EntCompania = Servicios_Medicos.Entities.Compania;

namespace ServiciosMedicos.Pages.Compania
{
    public class CrearModel : BasePageModel
    {
        protected override int? RolRequerido => 1;

        private readonly CompaniaService _service;

        [BindProperty]
        [Required(ErrorMessage = "El código de compañía es requerido.")]
        [MaxLength(50, ErrorMessage = "El código no puede superar 50 caracteres.")]
        public string CodigoCompania { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "El nombre es requerido.")]
        [MaxLength(150, ErrorMessage = "El nombre no puede superar 150 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        public string? ErrorMensaje { get; set; }

        public CrearModel(CompaniaService service)
        {
            _service = service;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var compania = new EntCompania
            {
                CodigoCompania = CodigoCompania.Trim(),
                Nombre = Nombre.Trim()
            };

            var (exito, mensaje) = await _service.Insertar(compania, UsuarioId);

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
