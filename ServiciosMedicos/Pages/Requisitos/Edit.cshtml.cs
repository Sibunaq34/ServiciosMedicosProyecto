using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;
using ServiciosMedicos.Pages;
namespace ServiciosMedicos.Pages.Requisitos
{
    public class EditModel : BasePageModel
    {
        private readonly IRequisitos _requisitosService;

        public EditModel(IRequisitos requisitosService)
        {
            _requisitosService = requisitosService;
        }

        [BindProperty]
        public RequisitoPuesto Requisito { get; set; } = new();

        [TempData]
        public string? Mensaje { get; set; }

        [TempData]
        public string? TipoMensaje { get; set; }

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

            try
            {
                var resultado =
                    await _requisitosService.ActualizarRequisito(Requisito);

                if (!resultado)
                {
                    ModelState.AddModelError("", "No se pudo actualizar el requisito");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }

            TipoMensaje = "success";
            Mensaje = "Requisito actualizado correctamente.";
            return RedirectToPage("Index", new { idPuesto = Requisito.IdPuesto });
        }
    }
}
