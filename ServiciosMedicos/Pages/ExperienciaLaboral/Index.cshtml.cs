using Microsoft.AspNetCore.Mvc;
using Servicios_Medicos.Services;
using EntExperiencia = Servicios_Medicos.Entities.ExperienciaLaboral;
using ServiciosMedicos.Pages;
namespace ServiciosMedicos.Pages.ExperienciaLaboral
{
    public class IndexModel : BasePageModel
    {
        protected override int? RolRequerido => 2;

        private readonly ExperienciaLaboralService _service;
        public const int TamanoPagina = 10;

        public List<EntExperiencia> Experiencias { get; set; } = new();
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public int OfertanteId { get; set; }
        public string NombreOferente { get; set; } = string.Empty;

        public IndexModel(ExperienciaLaboralService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync(int ofertanteId, int pagina = 1)
        {
            if (ofertanteId <= 0)
                return RedirectToPage("/Index");

            OfertanteId = ofertanteId;
            PaginaActual = pagina;

            NombreOferente = await _service.ObtenerNombreOferente(ofertanteId) ?? "Oferente";

            var (items, total) = await _service.ObtenerPaginado(ofertanteId, pagina, UsuarioId);
            Experiencias = items.ToList();
            TotalPaginas = (int)Math.Ceiling(total / (double)TamanoPagina);

            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id, int ofertanteId)
        {
            var (exito, mensaje) = await _service.Eliminar(id, UsuarioId);
            if (exito) TempData["Mensaje"] = mensaje; else TempData["Error"] = mensaje;
            return RedirectToPage(new { ofertanteId });
        }
    }
}
