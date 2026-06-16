using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Requisitos
{
    public class IndexModel : PageModel
    {
        private readonly IRequisitos _requisitosService;
        private const int TamanoPagina = 10;

        public IndexModel(IRequisitos requisitosService)
        {
            _requisitosService = requisitosService;
        }

        public IEnumerable<RequisitoPuesto> ListaRequisitos { get; set; } = [];
        public int IdPuesto { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }

        [TempData]
        public string? Mensaje { get; set; }

        [TempData]
        public string? TipoMensaje { get; set; }

        public async Task OnGet(int idPuesto, int pagina = 1)
        {
            IdPuesto = idPuesto;
            var requisitos = (await _requisitosService.ListarRequisitos(idPuesto)).ToList();

            TotalPaginas = (int)Math.Ceiling(requisitos.Count / (double)TamanoPagina);
            PaginaActual = Math.Clamp(pagina, 1, Math.Max(TotalPaginas, 1));

            ListaRequisitos = requisitos
                .Skip((PaginaActual - 1) * TamanoPagina)
                .Take(TamanoPagina);
        }

        public async Task<IActionResult> OnPostEliminar(int id, int idPuesto)
        {
            try
            {
                var resultado = await _requisitosService.EliminarRequisito(id);
                TipoMensaje = resultado ? "success" : "danger";
                Mensaje = resultado
                    ? "Requisito eliminado correctamente."
                    : "No se puede eliminar un registro con datos relacionados.";
            }
            catch
            {
                TipoMensaje = "danger";
                Mensaje = "No se puede eliminar un registro con datos relacionados.";
            }

            return RedirectToPage(new { idPuesto });
        }
    }
}
