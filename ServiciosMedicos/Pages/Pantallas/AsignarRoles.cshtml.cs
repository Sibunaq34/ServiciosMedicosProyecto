using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servicios_Medicos.Entities;
using Servicios_Medicos.Repository;
using ServiciosMedicos.Pages;

namespace ServiciosMedicos.Pages.Pantallas;

public class AsignarRolesModel : BasePageModel
{
    private readonly PantallasRepository _pantallasBD;

    public AsignarRolesModel(PantallasRepository pantallasBD)
    {
        _pantallasBD = pantallasBD;
    }

    [BindProperty]
    public int IdPantalla { get; set; }

    [BindProperty]
    public List<int> RolesSeleccionados { get; set; } = new();

    public IEnumerable<Rol> ListaRoles { get; set; }
        = new List<Rol>();

    public async Task OnGet(int idPantalla)
    {
        IdPantalla = idPantalla;

        ListaRoles =
            await _pantallasBD.ListarRoles();

        RolesSeleccionados =
            (await _pantallasBD.ListarRolesPorPantalla(idPantalla))
            .ToList();
    }

    public async Task<IActionResult> OnPost()
    {
        if (RolesSeleccionados == null ||
            !RolesSeleccionados.Any())
        {
            TempData["Validacion"] =
                "Debe seleccionar al menos un rol.";

            return RedirectToPage(new
            {
                idPantalla = IdPantalla
            });
        }

        try
        {
            await _pantallasBD.GuardarRolesPantalla(
                IdPantalla,
                RolesSeleccionados);

            TempData["Mensaje"] =
                "Roles asignados correctamente.";

            return RedirectToPage("Index");
        }
        catch
        {
            TempData["Error"] =
                "Ha ocurrido un error inesperado. Intente nuevamente.";

            return RedirectToPage(new
            {
                idPantalla = IdPantalla
            });
        }
    }

    public async Task<IActionResult> OnPostEliminarRoles()
    {
        try
        {
            RolesSeleccionados = new List<int>();

            await _pantallasBD.GuardarRolesPantalla(
                IdPantalla,
                RolesSeleccionados);

            TempData["Mensaje"] =
                "Roles asociados eliminados correctamente.";

            return RedirectToPage("Index");
        }
        catch
        {
            TempData["Error"] =
                "Ha ocurrido un error inesperado. Intente nuevamente.";

            return RedirectToPage(new
            {
                idPantalla = IdPantalla
            });
        }
    }
}