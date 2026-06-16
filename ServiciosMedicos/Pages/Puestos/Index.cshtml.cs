using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;

namespace Servicios_Medicos.Pages.Puestos
{
    public class IndexModel : PageModel
    {
        private readonly IPuestos _puestosService;
        private const int TamanoPagina = 10;

        public IndexModel(IPuestos puestosService)
        {
            _puestosService = puestosService;
        }

        public IEnumerable<Puesto> ListaPuestos { get; set; } = [];
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }

        [TempData]
        public string? Mensaje { get; set; }

        [TempData]
        public string? TipoMensaje { get; set; }

        public async Task OnGet(int pagina = 1)
        {
            var puestos = (await _puestosService.ListarPuestos()).ToList();

            TotalPaginas = (int)Math.Ceiling(puestos.Count / (double)TamanoPagina);
            PaginaActual = Math.Clamp(pagina, 1, Math.Max(TotalPaginas, 1));

            ListaPuestos = puestos
                .Skip((PaginaActual - 1) * TamanoPagina)
                .Take(TamanoPagina);
        }

        public async Task<IActionResult> OnPostEliminar(int id)
        {
            try
            {
                var resultado = await _puestosService.EliminarPuesto(id);
                TipoMensaje = resultado ? "success" : "danger";
                Mensaje = resultado
                    ? "Puesto eliminado correctamente."
                    : "No se puede eliminar un registro con datos relacionados.";
            }
            catch
            {
                TipoMensaje = "danger";
                Mensaje = "No se puede eliminar un registro con datos relacionados.";
            }

            return RedirectToPage();
        }
    }
}
