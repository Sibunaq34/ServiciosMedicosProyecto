using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;
using System.Text.RegularExpressions;
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
            ListaRoles = await _roles.Listar();
            LongitudUsuario = int.Parse(
                await _parametros.ObtenerValor("LONGITUD_USUARIO_EDIT"));

            if (string.IsNullOrWhiteSpace(Usuario.UsuarioNombre) ||
                string.IsNullOrWhiteSpace(Usuario.NombreCompleto) ||
                string.IsNullOrWhiteSpace(Usuario.Correo) ||
                Usuario.IdRol <= 0)
            {
                TempData["Validacion"] = "Debe completar todos los campos requeridos.";
                return Page();
            }

            if (!Regex.IsMatch(Usuario.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                TempData["Validacion"] = "Debe ingresar un correo electrónico válido.";
                return Page();
            }

            try
            {
                await _usuarios.Actualizar(Usuario);

                TempData["Mensaje"] = "Usuario actualizado correctamente.";
                return RedirectToPage("Index");
            }
            catch
            {
                TempData["Error"] = "No fue posible actualizar el usuario.";
                return Page();
            }
        }
    }
}