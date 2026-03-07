using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RegistroVisitas.Data;
using RegistroVisitas.Models;

namespace RegistroVisitas.Controllers
{
    public class VisitantesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hosting;

        public VisitantesController(ApplicationDbContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }

        public IActionResult Index()
        {
            var visitantes = _context.Visitantes.ToList();
            return View(visitantes);
        }

        public IActionResult Create()
        {
            ViewBag.Compañias = new SelectList(_context.Companias, "id_compañia", "nombre");
            ViewBag.Empleados = new SelectList(_context.Empleados, "id_empleado", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Visitante visitante, string? NuevaCompania, IFormFile? Foto)
        {
            visitante.nombre = visitante.nombre?.Trim();
            visitante.apellido_p = visitante.apellido_p?.Trim();
            visitante.apellido_m = visitante.apellido_m?.Trim();
            visitante.correo = visitante.correo?.Trim();

            using var tx = _context.Database.BeginTransaction();

            try
            {
                // 1️ Buscar visitante existente
                var visitanteDb = _context.Visitantes
                    .FirstOrDefault(v => v.correo == visitante.correo);

                int idVisitante;

                if (visitanteDb != null)
                {
                    idVisitante = visitanteDb.id_visitante;
                }
                else
                {
                    _context.Visitantes.Add(visitante);
                    _context.SaveChanges();
                    idVisitante = visitante.id_visitante;
                }

                // 2️ Validar visita activa
                bool visitaActiva = _context.Visitas
                    .Any(v => v.id_visitante == idVisitante && v.fecha_salida == null);

                if (visitaActiva)
                {
                    tx.Rollback();
                    ModelState.AddModelError("", "Este visitante ya se encuentra dentro.");
                    return View(visitante);
                }

                // 3️ Crear visita
                var visita = new Visita
                {
                    id_visitante = idVisitante,
                    fecha_entrada = DateTime.Now
                };

                _context.Visitas.Add(visita);
                _context.SaveChanges();

                tx.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                tx.Rollback();
                ModelState.AddModelError("", "Visitante duplicado detectado por la base de datos.");
                return View(visitante);
            }
        }

    }
}
