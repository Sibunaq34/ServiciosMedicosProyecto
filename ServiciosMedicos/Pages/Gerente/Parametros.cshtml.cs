using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Parametros
{
    public class ParametrosModel : PageModel
    {
        private readonly IParametro _parametro;

        public ParametrosModel(IParametro parametro)
        {
            _parametro = parametro;
        }

        [BindProperty]
        public ParametroEntidad Parametro { get; set; } = new();

        public IEnumerable<ParametroEntidad>
            ListaParametros
        { get; set; }
                = Enumerable.Empty<ParametroEntidad>();

        public string? Mensaje { get; set; }

        public async Task OnGetAsync()
        {
            ListaParametros =
                await _parametro.Listar();
        }
    }
}