using Microsoft.AspNetCore.Mvc;
using Servicios_Medicos.Services;
using EntCompania = Servicios_Medicos.Entities.Compania;
using ServiciosMedicos.Pages;
namespace ServiciosMedicos.Pages.Compania
{
    public class IndexModel : BasePageModel
    {
        protected override int? RolRequerido => 1;

        private readonly CompaniaService _service;
        public const int TamanoPagina = 10;

        public List<EntCompania> Companias { get; set; } = new();
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }

        public IndexModel(CompaniaService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync(int pagina = 1)
        {
            PaginaActual = pagina;
            var (items, total) = await _service.ObtenerPaginado(pagina, UsuarioId);
            Companias = items.ToList<EntCompania>();
            TotalPaginas = (int)Math.Ceiling(total / (double)TamanoPagina);
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var (exito, mensaje) = await _service.Eliminar(id, UsuarioId);
            if (exito) TempData["Mensaje"] = mensaje; else TempData["Error"] = mensaje;
            return RedirectToPage();
        }
    }
}
