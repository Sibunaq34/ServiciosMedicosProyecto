using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Services.Abstract;
using Microsoft.AspNetCore.Http;
namespace ServiciosMedicos.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUsuario _autenticacion;
        public LoginModel(IUsuario autenticacion)
        {
            _autenticacion = autenticacion;
        }

        [BindProperty]
        public string Usuario { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string? Error { get; set; }

        public bool Expirada { get; set; }
        public void OnGet(bool? expirada)
        {
            Expirada = expirada ?? false;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            HttpContext.Session.SetString("NombreUsuario", "admin");
            HttpContext.Session.SetString("NombreCompleto", "Administrador de prueba");
            HttpContext.Session.SetInt32("IdUsuario", 1);
            HttpContext.Session.SetInt32("IdRol", 1);
            HttpContext.Session.SetString("NombreRol", "Administrador");

            return RedirectToPage("/Index");

            /*
            var user =
                await _autenticacion.Login(
                    Usuario,
                    Password);

            if (user == null)
            {
                Error =
                    "Usuario o contraseña incorrectos";

                return Page();
            }

            HttpContext.Session.SetString(
                "NombreUsuario",
                user.Usuario);

            HttpContext.Session.SetInt32(
                "IdUsuario",
                user.IdUsuario);

            HttpContext.Session.SetInt32(
                "IdRol",
                user.IdRol);

            HttpContext.Session.SetString(
                "NombreRol",
                user.NombreRol ?? "");

            return RedirectToPage("/Index");
            */
        }

    }
}