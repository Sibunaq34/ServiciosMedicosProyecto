using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Requisitos
{
    public class IndexModel : PageModel
    {
        private const int TamanoPagina = 10;
        private readonly IRequisitos _requisitosService;

        public IndexModel(IRequisitos requisitosService)
        {
            _requisitosService = requisitosService;
        }

        [TempData]
        public string? Mensaje { get; set; }

        [TempData]
        public string? TipoMensaje { get; set; }

        public List<RequisitoPuesto> ListaRequisitos { get; set; } = [];
        public int IdPuesto { get; set; }
        public int PaginaActual { get; set; } = 1;
        public int TotalPaginas { get; set; } = 1;

        public async Task OnGet(int idPuesto, int pagina = 1)
        {
            IdPuesto = idPuesto;
            var requisitos = (await _requisitosService.ListarRequisitos(idPuesto)).ToList();
            TotalPaginas = (int)Math.Ceiling(requisitos.Count / (double)TamanoPagina);

            if (TotalPaginas == 0)
            {
                TotalPaginas = 1;
            }

            PaginaActual = Math.Clamp(pagina, 1, TotalPaginas);
            ListaRequisitos = requisitos
                .Skip((PaginaActual - 1) * TamanoPagina)
                .Take(TamanoPagina)
                .ToList();
        }

        public async Task<IActionResult> OnPostEliminar(int id, int idPuesto, int pagina = 1)
        {
            var exito = await _requisitosService.EliminarRequisito(id);

            TipoMensaje = exito ? "success" : "danger";
            Mensaje = exito
                ? "Requisito eliminado correctamente."
                : "No fue posible eliminar el requisito.";

            return RedirectToPage(new { idPuesto, pagina });
        }
    }
}
