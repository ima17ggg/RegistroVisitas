using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistroVisitas.Data;

namespace RegistroVisitas.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

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
            if (usuario == "vigilancia" && password == "admin1234")
            {
                HttpContext.Session.SetString("Vigilancia", "true");
                return RedirectToAction("Vigilancia_visita", "Vigilancia");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }

        // POST: /Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("Vigilancia") == "true")
            {
                var hoy = DateTime.Today;
                var mañana = hoy.AddDays(1);

                var visitasPendientes = _context.Visitas
                    .Where(v =>
                        v.fecha_salida == null &&
                        v.fecha_entrada >= hoy &&
                        v.fecha_entrada < mañana)
                    .ToList();

                foreach (var visita in visitasPendientes)
                {
                    visita.fecha_salida = DateTime.Now;
                }

                _context.SaveChanges();
            }

            HttpContext.Session.Clear();
              
            return RedirectToAction("Login", "Auth");
        }
    }
}
