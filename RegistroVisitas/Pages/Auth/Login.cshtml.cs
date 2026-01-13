using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace RegistroVisitas.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _config;

        public LoginModel(IConfiguration config)
        {
            _config = config;
        }

        [BindProperty]
        public string? Usuario { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public string? Error { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var expectedUser = _config["VigilanciaAuth:usuario"];
            var expectedPass = _config["VigilanciaAuth:password"];
    
            if (Usuario == expectedUser && Password == expectedPass)
            {
                // setear sesión/estado de auth si aplica
                return RedirectToPage("/Vigilancia/Index");
            }

            Error = "Usuario o contraseña inválidos.";
            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            // Clear auth cookie and any session
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();

            // Redirect to login page
            return RedirectToPage("/Auth/Login");
        }
    }
}           