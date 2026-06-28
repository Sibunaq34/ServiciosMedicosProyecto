using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiciosMedicos.Pages;
namespace ServiciosMedicos.Pages
{
    public class LogOutModel : BasePageModel
    {
        public async Task<IActionResult> OnGet()
        {
            HttpContext.Session.Clear();

            return RedirectToPage("/Login");
        }
    }
}