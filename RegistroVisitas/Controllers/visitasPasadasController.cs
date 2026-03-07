using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistroVisitas.Data;
using RegistroVisitas.Models.ViewModels;

namespace RegistroVisitas.Controllers
{
    public class visitasPasadasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public visitasPasadasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult VisitasPasadas()
        {
            if (HttpContext.Session.GetString("Vigilancia") != "true")
                return RedirectToAction("Login", "Auth");

            var visitantesActivos = _context.Visitas
               .Include(v => v.Visitante)
                   .ThenInclude(v => v.Compañia)
               .Include(v => v.Visitante)
                   .ThenInclude(v => v.Empleado)
               .Select(v => new VigilanciaViewModel
               {
                   id_visita = v.id_visita,
                   nombre = v.Visitante.nombre,
                   apellido_p = v.Visitante.apellido_p,
                   apellido_m = v.Visitante.apellido_m,
                   asunto = v.Visitante.asunto,
                   compania = v.Visitante.Compañia != null ? v.Visitante.Compañia.nombre : "N/A",
                   foto = v.Visitante.foto,
                   empleado = v.Visitante.Empleado != null ? v.Visitante.Empleado.nombre : "N/A",
                   fecha_entrada = v.fecha_entrada,
                   fecha_salida = v.fecha_salida
               })
               .ToList();

            // Usa ruta absoluta a la vista existente
            return View("~/Views/visitaPasadas/VisitasPasadas.cshtml", visitantesActivos);
        }

        public IActionResult regresar()
        {
            return RedirectToAction("Vigilancia_visita", "Vigilancia");
        }
    }
}
