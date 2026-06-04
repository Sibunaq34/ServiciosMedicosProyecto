using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiciosMedicos.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string? Usuario { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            Usuario =
                HttpContext.Session.GetString("NombreUsuario");

            if (string.IsNullOrEmpty(Usuario))
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }
    }
}