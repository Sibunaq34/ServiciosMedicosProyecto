using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;

namespace ServiciosMedicos.Pages.Parametross
{
    public class ParametrosModel : PageModel
    {
        private readonly IParametro _parametro;
        private const int TamanoPagina = 10;
        public ParametrosModel(IParametro parametro)
        {
            _parametro = parametro;
        }

        [BindProperty]
        public Parametros Parametro { get; set; } = new();

        public IReadOnlyList<Parametros> Parametro2 { get; set; } = Array.Empty<Parametros>();
        public IEnumerable<Parametros> ListaParametros
        { get; set; } = Enumerable.Empty<Parametros>();

        public string? Mensaje { get; set; }
        public int Pagina { get; set; } = 1;
        public bool HaySiguientePagina => Parametro2.Count == TamanoPagina;
        public async Task OnGetAsync(int? id, int pagina = 1)
        {
            Pagina = pagina < 1 ? 1 : pagina;

            try
            {
                Parametro2 = await _parametro.Listar(Pagina, TamanoPagina);


                if (id.HasValue)
                {
                    var parametro = await _parametro.ObtenerPorId(id.Value);

                    if (parametro != null)
                    {
                        Parametro = parametro;
                    }
                }
            }
            catch
            {
                Parametro2 = Array.Empty<Parametros>();
            }
        }

        public async Task<IActionResult> OnPostGuardar()
        {
            bool resultado;

            if (Parametro.IdParametro == 0)
            {

                int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

                if (!idUsuario.HasValue)
                {
                    return RedirectToPage("/Login");
                }
                await _parametro.Insertar(Parametro, idUsuario.Value);

                TempData["Mensaje"] =
                    $"Se registro correctamente";
            }
            else
            {
                int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

                if (!idUsuario.HasValue)
                {
                    return RedirectToPage("/Login");
                }
                resultado = await _parametro.Actualizar(Parametro, idUsuario.Value);

                TempData["Mensaje"] =
                    $"Se actualizo correctamente";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEliminar(int id)
        {
            try
            {
                int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

                if (!idUsuario.HasValue)
                {
                    return RedirectToPage("/Login");
                }

                await _parametro.Eliminar(id, idUsuario.Value);

                TempData["Mensaje"] =
                    "Parámetro eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToPage();
        }
    }
}