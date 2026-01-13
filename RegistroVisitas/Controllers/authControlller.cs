using Microsoft.AspNetCore.Mvc;

namespace RegistroVisitas.Controllers
{
    public class AuthController : Controller
    {
        // GET: /Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        public IActionResult Login(string usuario, string password)
        {
            // Credenciales simples (hardcodeadas)
            if (usuario == "vigilancia" && password == "1234")
            {
                // Guardamos sesión
                HttpContext.Session.SetString("Vigilancia", "true");

                return RedirectToAction("Vigilancia_visita", "Vigilancia");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Buena práctica de seguridad
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Borra todo
            HttpContext.Session.Remove("Vigilancia");
            return RedirectToAction("Login", "Auth");
        }
    }
}
