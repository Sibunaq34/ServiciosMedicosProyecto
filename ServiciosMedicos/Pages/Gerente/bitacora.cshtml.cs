using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;
using ServiciosMedicos.Entities;

namespace ServiciosMedicos.Pages.Gerente
{
    public class BitacoraModel : PageModel
    {
        private const int TamanoPagina = 10;
        private readonly IBitacora _bitacoraService;

        public BitacoraModel(IBitacora bitacoraService)
        {
            _bitacoraService = bitacoraService;
        }


        public IReadOnlyList<Bitacora> Bitacoras { get; set; } = Array.Empty<Bitacora>();


        public int Pagina { get; set; } = 1;


        public bool HaySiguientePagina =>Bitacoras.Count == TamanoPagina;


        [BindProperty(SupportsGet = true)]
        public string? Usuario { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Descripcion { get; set; }


        public async Task<IActionResult> OnGetAsync(int pagina = 1)
        {
            var usuarioSesion =
                HttpContext.Session.GetString("NombreUsuario");


            if (string.IsNullOrEmpty(usuarioSesion))
            {
                return RedirectToPage("/Login");
            }


            Pagina = pagina < 1 ? 1 : pagina;


            try
            {
                Bitacoras = await _bitacoraService.ConsultarBitacoras(Usuario,Descripcion,Pagina,TamanoPagina);
            }
            catch
            {
                Bitacoras =
                    Array.Empty<Bitacora>();
            }


            return Page();
        }
    }
}