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

        public IEnumerable<ParametroEntidad> ListaParametros
        { get; set; } = Enumerable.Empty<ParametroEntidad>();

        public string? Mensaje { get; set; }

        public async Task OnGetAsync(int? id)
        {
            ListaParametros =
                await _parametro.Listar();

            if (id.HasValue)
            {
                var parametro =
                    await _parametro.ObtenerPorId(id.Value);

                if (parametro != null)
                {
                    Parametro = parametro;
                }
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

                await _parametro.Eliminar(id,idUsuario.Value);

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