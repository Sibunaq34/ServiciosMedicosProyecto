using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Services.Abstract;
using ServiciosMedicos.Pages;
namespace ServiciosMedicos.Pages.Roles
{
    public class AsignarPantallasModel : BasePageModel
    {
        private readonly IRoles _roles;

        public AsignarPantallasModel(IRoles roles)
        {
            _roles = roles;
        }

        public int IdRol { get; set; }

        public IEnumerable<Pantalla> ListaPantallas { get; set; }
            = new List<Pantalla>();

        public async Task<IActionResult> OnGet(int idRol)
        {
            IdRol = idRol;

            ListaPantallas =
                await _roles.ListarPantallasPorRol(idRol);

            return Page();
        }
    }
}