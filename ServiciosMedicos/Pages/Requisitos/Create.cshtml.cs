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

            var resultado = await _requisitosService.InsertarRequisito(Requisito);
            if (!resultado)
            {
                ModelState.AddModelError("", "No se pudo guardar el requisito");
                return Page();
            }

            TempData["TipoMensaje"] = "success";
            TempData["Mensaje"] = "Requisito registrado correctamente.";
            return RedirectToPage("Index", new { idPuesto = Requisito.IdPuesto });
        }
    }
}
