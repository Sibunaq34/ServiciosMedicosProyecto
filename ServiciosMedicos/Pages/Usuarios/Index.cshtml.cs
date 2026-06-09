using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;

namespace ServiciosMedicos.Pages.Usuarios
{
    public class IndexModel : PageModel
    {
        private readonly IUsuariosAdmin _usuarios;

        public IndexModel(IUsuariosAdmin usuarios)
        {
            _usuarios = usuarios;
        }

        public IEnumerable<UsuarioAdmin> ListaUsuarios { get; set; }
            = new List<UsuarioAdmin>();

        public async Task OnGet()
        {
            ListaUsuarios = await _usuarios.Listar();
        }

        public async Task<IActionResult> OnPostEliminar(int idUsuario)
        {
            try
            {
                await _usuarios.Eliminar(idUsuario);

                TempData["Mensaje"] =
                    "Usuario eliminado correctamente.";
            }
            catch
            {
                TempData["Error"] =
                    "No se puede eliminar un usuario que tiene información relacionada.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCambiarEstado(
            int idUsuario,
            bool activo)
        {
            try
            {
                await _usuarios.CambiarEstado(
                    idUsuario,
                    !activo);

                TempData["Mensaje"] =
                    activo
                    ? "Usuario inactivado correctamente."
                    : "Usuario activado correctamente.";
            }
            catch
            {
                TempData["Error"] =
                    "No se pudo actualizar el estado del usuario.";
            }

            return RedirectToPage();
        }
    }
}