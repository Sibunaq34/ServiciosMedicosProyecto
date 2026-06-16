using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Requisitos
{
    public class CreateModel : PageModel
    {
        private readonly IRequisitos _requisitosService;

        public CreateModel(IRequisitos requisitosService)
        {
            _requisitosService = requisitosService;
        }

        [BindProperty]
        public RequisitoPuesto Requisito { get; set; } = new();

        public void OnGet(int idPuesto)
        {
            Requisito.IdPuesto = idPuesto;
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            await _requisitosService.InsertarRequisito(Requisito);
            return RedirectToPage("Index", new { idPuesto = Requisito.IdPuesto });
        }
    }
}
