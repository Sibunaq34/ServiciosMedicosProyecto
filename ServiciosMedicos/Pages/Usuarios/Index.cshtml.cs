using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;
using ServiciosMedicos.Pages;
namespace ServiciosMedicos.Pages.Usuarios
{
    public class IndexModel : BasePageModel
    {
        private readonly IUsuariosAdmin _usuarios;

        public IndexModel(IUsuariosAdmin usuarios)
        {
            _usuarios = usuarios;
        }

        public IEnumerable<UsuarioAdmin> ListaUsuarios { get; set; }
            = new List<UsuarioAdmin>();

        [BindProperty(SupportsGet = true)]
        public int Pagina { get; set; } = 1;

        public int TotalPaginas { get; set; }

        private const int TamanoPagina = 10;

        public async Task OnGet()
        {
            var usuarios = (await _usuarios.Listar()).ToList();

            TotalPaginas = (int)Math.Ceiling(
                usuarios.Count / (double)TamanoPagina);

            if (TotalPaginas == 0)
                TotalPaginas = 1;

            if (Pagina < 1)
                Pagina = 1;

            if (Pagina > TotalPaginas)
                Pagina = TotalPaginas;

            ListaUsuarios = usuarios
                .Skip((Pagina - 1) * TamanoPagina)
                .Take(TamanoPagina)
                .ToList();
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
                    "No se puede eliminar un registro con datos relacionados.";
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
                    "Ha ocurrido un error inesperado. Intente nuevamente.";
            }

            return RedirectToPage();
        }
    }
}