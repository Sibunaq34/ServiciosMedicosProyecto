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
            ListaUsuarios =
                await _usuarios.Listar();
        }

        public async Task<IActionResult>
            OnPostEliminar(int idUsuario)
        {
            try
            {
                await _usuarios.Eliminar(idUsuario);

                TempData["Mensaje"] =
                    "Usuario eliminado correctamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    ex.Message;
            }

            return RedirectToPage();
        }

        public async Task<IActionResult>
            OnPostCambiarEstado(
                int idUsuario,
                bool activo)
        {
            try
            {
                await _usuarios.CambiarEstado(
                    idUsuario,
                    !activo);

                TempData["Mensaje"] =
                    "Estado actualizado correctamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    ex.Message;
            }

            return RedirectToPage();
        }
    }
}