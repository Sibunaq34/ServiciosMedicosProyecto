using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;
using System.Text.RegularExpressions;
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
            ListaRoles = await _roles.Listar();
            LongitudUsuario = int.Parse(
                await _parametros.ObtenerValor("LONGITUD_USUARIO"));

            if (string.IsNullOrWhiteSpace(Usuario.UsuarioNombre) ||
                string.IsNullOrWhiteSpace(Usuario.NombreCompleto) ||
                string.IsNullOrWhiteSpace(Usuario.Correo) ||
                string.IsNullOrWhiteSpace(Usuario.Contrasena) ||
                Usuario.IdRol <= 0 ||
                string.IsNullOrWhiteSpace(Usuario.Estado))
            {
                TempData["Validacion"] = "Debe completar todos los campos requeridos.";
                return Page();
            }

            if (!Regex.IsMatch(Usuario.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                TempData["Validacion"] = "Debe ingresar un correo electrónico válido.";
                return Page();
            }

            if (!Regex.IsMatch(Usuario.Contrasena, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$"))
            {
                TempData["Validacion"] = "La contraseña debe contener al menos 8 caracteres, una letra mayúscula, una letra minúscula, un número y un carácter especial.";
                return Page();
            }

            try
            {
                await _usuarios.Crear(Usuario);

                TempData["Mensaje"] = "Usuario creado correctamente.";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                var mensaje = ex.Message.ToLower();

                if (mensaje.Contains("usuario"))
                {
                    TempData["Error"] = "El nombre de usuario ya existe.";
                }
                else if (mensaje.Contains("correo") || mensaje.Contains("email"))
                {
                    TempData["Error"] = "Ya existe un usuario registrado con ese correo.";
                }
                else
                {
                    TempData["Error"] = "Ha ocurrido un error inesperado. Intente nuevamente.";
                }

                return Page();
            }
        }
    }
}