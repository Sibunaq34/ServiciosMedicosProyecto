using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiciosMedicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Gerente
{
    public class BitacoraModel : PageModel
    {
        private readonly IBitacora _bitacoraService;

        public List<Bitacora> Bitacoras { get; set; } = new();

        public string? UsuarioSesion { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Usuario { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Descripcion { get; set; }

        public BitacoraModel(IBitacora bitacoraService)
        {
            _bitacoraService = bitacoraService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            UsuarioSesion =
                HttpContext.Session.GetString("NombreUsuario");

            if (string.IsNullOrEmpty(UsuarioSesion))
            {
                return RedirectToPage("/Login");
            }

            Bitacoras = (await _bitacoraService
                .ConsultarBitacoras(
                    Usuario,
                    Descripcion))
                .ToList();

            return Page();
        }
    }
}