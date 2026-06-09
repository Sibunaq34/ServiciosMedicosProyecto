using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Requisitos
{
    public class EditModel : PageModel
    {
        private readonly IRequisitos _requisitosService;

        public EditModel(IRequisitos requisitosService)
        {
            _requisitosService = requisitosService;
        }

        [BindProperty]
        public RequisitoPuesto Requisito { get; set; } = new();

        public void OnGet(int id, string nombre, int idPuesto)
        {
            Requisito = new RequisitoPuesto
            {
                IdRequisito = id,
                NombreRequisito = nombre,
                IdPuesto = idPuesto
            };
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            await _requisitosService.ActualizarRequisito(Requisito);
            return RedirectToPage("Index", new { idPuesto = Requisito.IdPuesto });
        }
    }
}
