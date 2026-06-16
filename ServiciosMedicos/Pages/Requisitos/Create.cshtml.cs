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

        [TempData]
        public string? Mensaje { get; set; }

        [TempData]
        public string? TipoMensaje { get; set; }

        public void OnGet(int idPuesto)
        {
            Requisito.IdPuesto = idPuesto;
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var resultado =
                    await _requisitosService.InsertarRequisito(Requisito);

                if (!resultado)
                {
                    ModelState.AddModelError("", "No se pudo guardar el requisito");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }

            TipoMensaje = "success";
            Mensaje = "Requisito creado correctamente.";
            return RedirectToPage("Index", new { idPuesto = Requisito.IdPuesto });
        }
    }
}
