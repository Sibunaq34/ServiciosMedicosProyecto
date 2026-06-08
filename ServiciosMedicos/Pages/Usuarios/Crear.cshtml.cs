using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Usuarios
{
    public class CrearModel : PageModel
    {
        private readonly IUsuariosAdmin _usuarios;
        private readonly IRoles _roles;
        private readonly IParametro _parametros;

        public CrearModel(
            IUsuariosAdmin usuarios,
            IRoles roles,
            IParametro parametros)
        {
            _usuarios = usuarios;
            _roles = roles;
            _parametros = parametros;
        }

        [BindProperty]
        public UsuarioAdmin Usuario { get; set; } = new();

        public IEnumerable<Rol> ListaRoles { get; set; } = new List<Rol>();
        public int LongitudUsuario { get; set; }

        public async Task OnGet()
        {
            Usuario.Activo = true;
            Usuario.Estado = "Activo";

            ListaRoles = await _roles.Listar();
            LongitudUsuario = int.Parse(
                await _parametros.ObtenerValor("LONGITUD_USUARIO"));
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                await _usuarios.Crear(Usuario);

                TempData["Mensaje"] =
                    "Usuario creado correctamente";

                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ListaRoles = await _roles.Listar();
                LongitudUsuario = int.Parse(
                    await _parametros.ObtenerValor("LONGITUD_USUARIO"));
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return Page();
            }

        }
    }
}