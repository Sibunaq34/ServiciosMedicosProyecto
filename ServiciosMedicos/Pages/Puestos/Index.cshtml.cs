using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;

namespace Servicios_Medicos.Pages.Puestos
{
    public class IndexModel : PageModel
    {
        private const int TamanoPagina = 10;
        private readonly IPuestos _puestosService;

        public IndexModel(IPuestos puestosService)
        {
            _puestosService = puestosService;
        }

        [TempData]
        public string? Mensaje { get; set; }

        [TempData]
        public string? TipoMensaje { get; set; }

        public List<Puesto> ListaPuestos { get; set; } = [];
        public int PaginaActual { get; set; } = 1;
        public int TotalPaginas { get; set; } = 1;

        public async Task OnGet(int pagina = 1)
        {
            var puestos = (await _puestosService.ListarPuestos()).ToList();
            TotalPaginas = (int)Math.Ceiling(puestos.Count / (double)TamanoPagina);

            if (TotalPaginas == 0)
            {
                TotalPaginas = 1;
            }

            PaginaActual = Math.Clamp(pagina, 1, TotalPaginas);
            ListaPuestos = puestos
                .Skip((PaginaActual - 1) * TamanoPagina)
                .Take(TamanoPagina)
                .ToList();
        }

        public async Task<IActionResult> OnPostEliminar(int id, int pagina = 1)
        {
            var exito = await _puestosService.EliminarPuesto(id);

            TipoMensaje = exito ? "success" : "danger";
            Mensaje = exito
                ? "Puesto eliminado correctamente."
                : "No fue posible eliminar el puesto.";

            return RedirectToPage(new { pagina });
        }
    }
}
