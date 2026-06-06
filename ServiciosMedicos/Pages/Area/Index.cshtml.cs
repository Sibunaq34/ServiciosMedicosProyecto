using Microsoft.AspNetCore.Mvc;
using Servicios_Medicos.Services;
using EntArea = Servicios_Medicos.Entities.Area;

namespace ServiciosMedicos.Pages.Area
{
    public class IndexModel : BasePageModel
    {
        protected override int? RolRequerido => 2;

        private readonly AreaService _service;
        public const int TamanoPagina = 10;

        [TempData] public string? Mensaje { get; set; }
        [TempData] public string? TipoMensaje { get; set; }

        public List<EntArea> Areas { get; set; } = new();
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }

        public IndexModel(AreaService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync(int pagina = 1)
        {
            PaginaActual = pagina;
            var (items, total) = await _service.ObtenerPaginado(pagina, UsuarioId);
            Areas = items.ToList();
            TotalPaginas = (int)Math.Ceiling(total / (double)TamanoPagina);
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var (exito, mensaje) = await _service.Eliminar(id, UsuarioId);
            TipoMensaje = exito ? "success" : "danger";
            Mensaje = mensaje;
            return RedirectToPage();
        }
    }
}
