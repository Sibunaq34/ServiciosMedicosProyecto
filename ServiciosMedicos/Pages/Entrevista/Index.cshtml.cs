using Microsoft.AspNetCore.Mvc;
using Servicios_Medicos.Services;
using EntEntrevista = Servicios_Medicos.Entities.Entrevista;
using ServiciosMedicos.Pages;
namespace ServiciosMedicos.Pages.Entrevista
{
    public class IndexModel : BasePageModel
    {
        protected override int? RolRequerido => 2;

        private readonly EntrevistaService _service;
        public const int TamanoPagina = 10;

        public List<EntEntrevista> Entrevistas { get; set; } = new();
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }

        public IndexModel(EntrevistaService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync(int pagina = 1)
        {
            PaginaActual = pagina;
            var (items, total) = await _service.ObtenerPaginado(pagina, UsuarioId);
            Entrevistas = items.ToList();
            TotalPaginas = (int)Math.Ceiling(total / (double)TamanoPagina);
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var (exito, mensaje) = await _service.Eliminar(id, UsuarioId);
            if (exito) TempData["Mensaje"] = mensaje; else TempData["Error"] = mensaje;
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostMarcarRealizadaAsync(int id)
        {
            var (exito, mensaje) = await _service.MarcarRealizada(id, UsuarioId);
            if (exito) TempData["Mensaje"] = mensaje; else TempData["Error"] = mensaje;
            return RedirectToPage();
        }
    }
}
