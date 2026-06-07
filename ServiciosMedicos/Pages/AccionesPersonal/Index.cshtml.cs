using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace Servicios_Medicos.Pages.AccionesPersonal
{
    public class IndexModel : PageModel
    {
        private readonly IAccionesPersonal _service;

        public IndexModel(
            IAccionesPersonal service)
        {
            _service = service;
        }

        public IEnumerable<AccionPersonal>
            ListaAcciones
        { get; set; }
            = [];

        public async Task OnGet()
        {
            ListaAcciones =
                await _service.ListarAcciones();
        }
    }
}
