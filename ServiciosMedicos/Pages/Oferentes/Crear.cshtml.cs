using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;
using System.ComponentModel.DataAnnotations;

namespace ServiciosMedicos.Pages.Oferentes
{
    // Persona C - Kenneth: Creacion de oferentes OFE1.
    public class CrearModel : PageModel
    {
        private readonly IOferenteService _oferenteService;
        private readonly IConcursoService _concursoService;

        public CrearModel(
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

        public async Task<IActionResult> OnGetAsync()
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            await CargarConcursosAsync(idUsuario.Value);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return RedirigirALogin();

            PrepararYValidarFormulario();

            if (!ModelState.IsValid)
            {
                await CargarConcursosAsync(idUsuario.Value);
                return Page();
            }

            try
            {
                await _oferenteService.CrearAsync(
                    Oferente,
                    idUsuario.Value);

                TempData["MensajeExito"] =
                    "El oferente fue registrado correctamente.";

                return RedirectToPage("./Index");
            }
            catch (InvalidOperationException ex)
            {
                MensajeError = ex.Message;
            }
            catch
            {
                MensajeError =
                    "Ocurrio un error al registrar el oferente.";
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

        private void PrepararYValidarFormulario()
        {
            Oferente.Correos = SepararLineas(CorreosTexto);
            Oferente.Telefonos = SepararLineas(TelefonosTexto);
            Oferente.ConcursosIds = ConcursosSeleccionados;

            ModelState.Clear();
            TryValidateModel(Oferente, nameof(Oferente));

            if (Oferente.Correos.Count == 0)
            {
                ModelState.AddModelError(
                    nameof(CorreosTexto),
                    "Debe indicar al menos un correo electronico.");
            }

            if (Oferente.Telefonos.Count == 0)
            {
                ModelState.AddModelError(
                    nameof(TelefonosTexto),
                    "Debe indicar al menos un telefono.");
            }
            else
            {
                foreach (var telefono in Oferente.Telefonos)
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(
                            telefono,
                            @"^\d{8}$"))
                    {
                        ModelState.AddModelError(
                            nameof(TelefonosTexto),
                            "El teléfono debe contener exactamente 8 dígitos numéricos.");
                    }
                }
            }

            if (Oferente.ConcursosIds.Count == 0)
            {
                ModelState.AddModelError(
                    nameof(ConcursosSeleccionados),
                    "Debe seleccionar al menos un concurso.");
            }

            var emailValidator = new EmailAddressAttribute();

            foreach (var correo in Oferente.Correos)
            {
                if (!emailValidator.IsValid(correo))
                {
                    ModelState.AddModelError(
                        nameof(CorreosTexto),
                        $"El correo '{correo}' no tiene un formato valido.");
                }
            }
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
