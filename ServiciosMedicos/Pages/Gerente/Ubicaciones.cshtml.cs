using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Ubicaciones
{
    public class UbicacionesModel : PageModel
    {
        private readonly IUbicacion _ubicacionService;

        public UbicacionesModel(
            IUbicacion ubicacionService)
        {
            _ubicacionService = ubicacionService;
        }

        [BindProperty]
        public IFormFile? Archivo { get; set; }

        public string? Mensaje { get; set; }

        public string? Error { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (Archivo == null)
                {
                    Error =
                        "Debe seleccionar un archivo.";

                    return Page();
                }

                await _ubicacionService
                    .CargarUbicaciones(Archivo);

                Mensaje =
                    "Ubicaciones cargadas correctamente.";

                return Page();
            }
            catch (Exception ex)
            {
                Error = ex.Message;

                return Page();
            }
        }
    }
}