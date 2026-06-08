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

        public CrearModel(
            IUsuariosAdmin usuarios,
            IRoles roles)
        {
            _usuarios = usuarios;
            _roles = roles;
        }

        [BindProperty]
        public UsuarioAdmin Usuario { get; set; } = new();

        public IEnumerable<Rol> ListaRoles { get; set; } = new List<Rol>();

        public async Task OnGet()
        {
            Usuario.Activo = true;
            Usuario.Estado = "Activo";

            ListaRoles = await _roles.Listar();
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

                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return Page();
            }
        }
    }
}