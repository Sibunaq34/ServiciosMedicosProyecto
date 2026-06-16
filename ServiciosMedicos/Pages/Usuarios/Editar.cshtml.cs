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
        private readonly IParametro _parametros;

        public EditarModel(
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
            LongitudUsuario = int.Parse(
                await _parametros.ObtenerValor("LONGITUD_USUARIO_EDIT"));

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
                LongitudUsuario = int.Parse(
                  await _parametros.ObtenerValor("LONGITUD_USUARIO_EDIT"));
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return Page();
            }
        }
    }
}