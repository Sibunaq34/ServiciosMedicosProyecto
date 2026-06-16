using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;

namespace ServiciosMedicos.Pages.Pantallas;

public class AsignarRolesModel : PageModel
{
    private readonly PantallasBD _pantallasBD;

    public AsignarRolesModel(PantallasBD pantallasBD)
    {
        _pantallasBD = pantallasBD;
    }

    [BindProperty]
    public int IdPantalla { get; set; }

    [BindProperty]
    public List<int> RolesSeleccionados { get; set; } = new();

    public IEnumerable<Rol> ListaRoles { get; set; } = new List<Rol>();

    public async Task OnGet(int idPantalla)
    {
        IdPantalla = idPantalla;

        ListaRoles = await _pantallasBD.ListarRoles();

        RolesSeleccionados =
            (await _pantallasBD.ListarRolesPorPantalla(idPantalla))
            .ToList();
    }

    public async Task<IActionResult> OnPost()
    {
        await _pantallasBD.GuardarRolesPantalla(
            IdPantalla,
            RolesSeleccionados);

        TempData["Mensaje"] = "Roles asignados correctamente.";

        return RedirectToPage("Index");
    }
}