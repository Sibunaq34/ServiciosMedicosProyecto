using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;
using ServiciosMedicos.Pages;

namespace ServiciosMedicos.Pages.Oferentes
{
    // Persona C - Kenneth: Edicion con datos precargados para OFE1.
    public class EditarModel : BasePageModel
    {
        private readonly IOferenteService _oferenteService;
        private readonly IConcursoService _concursoService;

        public EditarModel(
            IOferenteService oferenteService,
            IConcursoService concursoService)
        {
            _oferenteService = oferenteService;
            _concursoService = concursoService;
        }

        [BindProperty]
        public Oferente Oferente { get; set; } =
            new();

        [BindProperty]
        public string? CorreosTexto { get; set; }

        [BindProperty]
        public string? TelefonosTexto { get; set; }

        [BindProperty]
        public List<int> ConcursosSeleccionados { get; set; } =
            new();

        public IReadOnlyList<Concurso> ConcursosDisponibles { get; set; } =
            Array.Empty<Concurso>();

        public string? MensajeError { get; set; }

        public async Task<IActionResult> OnGetAsync(
            int id)
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            try
            {
                var oferente =
                    await _oferenteService.ObtenerAsync(
                        id,
                        idUsuario.Value);

                if (oferente == null)
                {
                    TempData["MensajeError"] =
                        "El oferente no existe.";

                    return RedirectToPage("./Index");
                }

                Oferente = oferente;
                CorreosTexto = string.Join(Environment.NewLine, oferente.Correos);
                TelefonosTexto = string.Join(Environment.NewLine, oferente.Telefonos);
                ConcursosSeleccionados = oferente.ConcursosIds;
            }
            catch (InvalidOperationException ex)
            {
                TempData["MensajeError"] = ex.Message;

                return RedirectToPage("./Index");
            }
            catch
            {
                TempData["MensajeError"] =
                    "Ocurrio un error al consultar el oferente.";

                return RedirectToPage("./Index");
            }

            await CargarConcursosAsync(idUsuario.Value);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(
            int id)
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            Oferente.IdOferente = id;
            PrepararFormulario();

            try
            {
                await _oferenteService.ActualizarAsync(
                    Oferente,
                    idUsuario.Value);

                TempData["MensajeExito"] =
                    "El oferente fue actualizado correctamente.";

                return RedirectToPage("./Index");
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
            }
            catch
            {
                MensajeError =
                    "Ocurrio un error al actualizar el oferente.";
            }

            await CargarConcursosAsync(idUsuario.Value);
            return Page();
        }

        private async Task CargarConcursosAsync(
            int idUsuario)
        {
            try
            {
                ConcursosDisponibles =
                    await _concursoService.ListarAsync(
                        1,
                        100,
                        idUsuario);
            }
            catch
            {
                ConcursosDisponibles = Array.Empty<Concurso>();
                MensajeError ??=
                    "Ocurrio un error al cargar los concursos disponibles.";
            }
        }

        private void PrepararFormulario()
        {
            Oferente.Correos = SepararLineas(CorreosTexto);
            Oferente.Telefonos = SepararLineas(TelefonosTexto);
            Oferente.ConcursosIds = ConcursosSeleccionados;
        }

        private static List<string> SepararLineas(
            string? texto)
        {
            return (texto ?? string.Empty)
                .Split(
                    new[] { "\r\n", "\n", ";", "," },
                    StringSplitOptions.RemoveEmptyEntries)
                .Select(valor => valor.Trim())
                .Where(valor => !string.IsNullOrWhiteSpace(valor))
                .ToList();
        }

        private int? ObtenerIdUsuario()
        {
            return HttpContext.Session.GetInt32("IdUsuario");
        }

        private IActionResult RedirigirALogin()
        {
            return RedirectToPage(
                "/LogIn",
                new { expirada = true });
        }
    }
}
