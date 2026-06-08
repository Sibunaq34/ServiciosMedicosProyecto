using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Usuarios
{
    public class EditarModel : PageModel
    {
        private readonly IUsuariosAdmin _usuarios;
        private readonly IRoles _roles;

        public EditarModel(
            IUsuariosAdmin usuarios,
            IRoles roles)
        {
            _usuarios = usuarios;
            _roles = roles;
        }

        [BindProperty]
        public UsuarioAdmin Usuario { get; set; } = new();

        public IEnumerable<Rol> ListaRoles { get; set; } = new List<Rol>();

        public async Task<IActionResult> OnGet(int idUsuario)
        {
            var usuario = await _usuarios.ObtenerPorId(idUsuario);

            if (usuario == null)
            {
                return RedirectToPage("Index");
            }

            Usuario = usuario;
            Usuario.Contrasena = string.Empty;

            ListaRoles = await _roles.Listar();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                await _usuarios.Actualizar(Usuario);

                TempData["Mensaje"] =
                    "Usuario actualizado correctamente.";

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