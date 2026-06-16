using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;

namespace Servicios_Medicos.Pages.AccionesPersonal
{
    public class IndexModel : PageModel
    {
        private readonly IAccionesPersonal _service;
        private const int TamanoPagina = 10;

        public IndexModel(
            IAccionesPersonal service)
        {
            _service = service;
        }

        public IEnumerable<AccionPersonal>
            ListaAcciones
        { get; set; }
            = [];

        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }

        [TempData]
        public string? Mensaje { get; set; }

        [TempData]
        public string? TipoMensaje { get; set; }

        public async Task OnGet(int pagina = 1)
        {
            var acciones =
                (await _service.ListarAcciones()).ToList();

            TotalPaginas =
                (int)Math.Ceiling(acciones.Count / (double)TamanoPagina);

            PaginaActual =
                Math.Clamp(pagina, 1, Math.Max(TotalPaginas, 1));

            ListaAcciones =
                acciones
                    .Skip((PaginaActual - 1) * TamanoPagina)
                    .Take(TamanoPagina);
        }
    }
}
