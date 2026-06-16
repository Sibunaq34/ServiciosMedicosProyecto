using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Requisitos
{
    public class IndexModel : PageModel
    {
        private readonly IRequisitos _requisitosService;

        public IndexModel(IRequisitos requisitosService)
        {
            _requisitosService = requisitosService;
        }

        public IEnumerable<RequisitoPuesto> ListaRequisitos { get; set; } = [];
        public int IdPuesto { get; set; }

        public async Task OnGet(int idPuesto)
        {
            IdPuesto = idPuesto;
            ListaRequisitos = await _requisitosService.ListarRequisitos(idPuesto);
        }

        public async Task<IActionResult> OnPostEliminar(int id, int idPuesto)
        {
            await _requisitosService.EliminarRequisito(id);
            return RedirectToPage(new { idPuesto });
        }
    }
}
