using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiciosMedicos.Pages
{
    public class BasePageModel : PageModel
    {
        protected int UsuarioId => HttpContext.Session.GetInt32("IdUsuario") ?? 0;
        protected string UsuarioNombre => HttpContext.Session.GetString("NombreUsuario") ?? "";
        protected int UsuarioRol => HttpContext.Session.GetInt32("IdRol") ?? 0;

        // Las páginas que requieran un rol específico sobreescriben esta propiedad.
        // null = cualquier usuario autenticado puede acceder.
        protected virtual int? RolRequerido => null;

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("NombreUsuario")))
            {
                context.Result = RedirectToPage("/LogIn",
                    new { mensaje = "Por favor inicie sesión para utilizar el sistema" });
                return;
            }

            if (RolRequerido.HasValue && UsuarioRol != RolRequerido.Value)
            {
                context.Result = RedirectToPage("/Index");
                return;
            }

            base.OnPageHandlerExecuting(context);
        }
    }
}
