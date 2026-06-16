using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;
using ServiciosMedicos.Pages;
namespace ServiciosMedicos.Pages
{
    public class LoginModel : BasePageModel
    {
        private readonly IUsuario _autenticacion;
        private readonly PantallasRepository _pantallasBD;

        public LoginModel(
            IUsuario autenticacion,
            PantallasRepository pantallasBD)
        {
            _autenticacion = autenticacion;
            _pantallasBD = pantallasBD;
        }

        [BindProperty]
        public string Usuario { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string? Error { get; set; }

        public bool Expirada { get; set; }

        public void OnGet(bool expirada = false)
        {
            Expirada = expirada;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var user = await _autenticacion.Login(Usuario, Password);

                if (user == null)
                {
                    Error =
                        "Usuario o contraseña incorrectos";

                    return Page();
                }
            if (user == null)
            {
                Error = "Usuario o contraseña incorrectos";
                return Page();
            }

                HttpContext.Session.SetString("NombreUsuario", user.Usuario);
                HttpContext.Session.SetString("NombreCompleto", user.Usuario);

                HttpContext.Session.SetInt32("IdUsuario", user.IdUsuario);
            HttpContext.Session.SetString("NombreUsuario", user.Usuario ?? "");
            HttpContext.Session.SetString("NombreCompleto", user.NombreCompleto ?? "");
            HttpContext.Session.SetInt32("IdUsuario", user.IdUsuario);
            HttpContext.Session.SetInt32("IdRol", user.IdRol);
            HttpContext.Session.SetString("NombreRol", user.NombreRol ?? "");

                HttpContext.Session.SetInt32("IdRol", user.IdRol);
            var pantallasRol =
                await _pantallasBD.ListarNombresPantallasPorRol(user.IdRol);

                HttpContext.Session.SetString("NombreRol", user.NombreRol ?? "");
            HttpContext.Session.SetString(
                "PantallasRol",
                string.Join("|", pantallasRol)
            );

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}