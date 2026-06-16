using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;
using ServiciosMedicos.Pages;
namespace Servicios_Medicos.Pages.AccionesPersonal
{
    public class IndexModel : BasePageModel
    {
        private const int TamanoPagina = 10;
        private readonly IAccionesPersonal _service;

        public IndexModel(
            IAccionesPersonal service)
        {
            _service = service;
        }

        [TempData]
        public string? Mensaje { get; set; }

        [TempData]
        public string? TipoMensaje { get; set; }

        public List<AccionPersonal>
            ListaAcciones
        { get; set; }
            = [];

        public int PaginaActual { get; set; } = 1;

        public int TotalPaginas { get; set; } = 1;

        public async Task OnGet(int pagina = 1)
        {
            var acciones =
                (await _service.ListarAcciones()).ToList();

            TotalPaginas =
                (int)Math.Ceiling(
                    acciones.Count / (double)TamanoPagina);

            if (TotalPaginas == 0)
            {
                TotalPaginas = 1;
            }

            PaginaActual =
                Math.Clamp(pagina, 1, TotalPaginas);

            ListaAcciones = acciones
                .Skip((PaginaActual - 1) * TamanoPagina)
                .Take(TamanoPagina)
                .ToList();
        }

        public async Task<IActionResult> OnPostEliminar(
            int id,
            int pagina = 1)
        {
            var exito =
                await _service.EliminarAccion(id);

            TipoMensaje = exito ? "success" : "danger";
            Mensaje = exito
                ? "Acción de personal eliminada correctamente."
                : "No fue posible eliminar la acción de personal.";

            return RedirectToPage(new { pagina });
        }
    }
}
