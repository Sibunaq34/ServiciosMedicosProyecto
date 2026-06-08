using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiciosMedicos.Entities;
using ServiciosMedicos.Services.Abstract;

namespace Servicios_Medicos.Pages.Puestos
{
    public class IndexModel : PageModel
    {
        private readonly IPuestos _puestosService;

        public IndexModel(IPuestos puestosService)
        {
            _puestosService = puestosService;
        }

        public IEnumerable<Puesto> ListaPuestos { get; set; } = [];

        public async Task OnGet()
        {
            ListaPuestos = await _puestosService.ListarPuestos();
        }
    }
}
