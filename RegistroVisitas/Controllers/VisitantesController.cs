using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            ViewBag.Compañias = new SelectList(
                _context.Companias,
                "id_compañia",
                "nombre"
            );

            ViewBag.Empleados = new SelectList(
                _context.Empleados,
                "id_empleado",
                "nombre"
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Visitante visitante, string? NuevaCompania, IFormFile? Foto)
        {
                // Normaliza: trata null como 0
                if (!visitante.id_compañia.HasValue) visitante.id_compañia = 0;

                // Si es 0 y hay NuevaCompania, crea la compañía
                if (visitante.id_compañia == 0 && !string.IsNullOrWhiteSpace(NuevaCompania))
                {
                    var compania = new Compañia { nombre = NuevaCompania.Trim() };
                    _context.Companias.Add(compania);
                    _context.SaveChanges();
                    visitante.id_compañia = compania.id_compañia;
                }

                // Validación adicional: si después de todo queda <=0, error
                if (!visitante.id_compañia.HasValue || visitante.id_compañia <= 0)
                {
                    ModelState.AddModelError("id_compañia", "Seleccione una compañía válida o ingrese el nombre de la nueva.");
                }

                // Ahora valida ModelState
                if (!ModelState.IsValid)
                {
                    ViewBag.Compañias = new SelectList(_context.Companias, "id_compañia", "nombre");
                    ViewBag.Empleados = new SelectList(_context.Empleados, "id_empleado", "nombre");
                    return View(visitante);
                }

            // Validar ahora que ya se resolvió la compañía
            if (!ModelState.IsValid)
            {
                ViewBag.Compañias = new SelectList(_context.Companias, "id_compañia", "nombre");
                ViewBag.Empleados = new SelectList(_context.Empleados, "id_empleado", "nombre");
                return View(visitante);
            }

            if (!ModelState.IsValid) { /* ... */ }

            // Manejo de foto
            if (Foto != null && Foto.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Foto.FileName);
                var uploadsFolder = Path.Combine(_hosting.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Foto.CopyTo(stream);
                }
                visitante.foto = "/images/" + fileName;  // Guarda la ruta relativa
            }

            // Insertar visitante y visita en una sola unidad de trabajo
            using var tx = _context.Database.BeginTransaction();
            try
            {
                _context.Visitantes.Add(visitante);
                _context.SaveChanges();

                _context.Visitas.Add(new Visita
                {
                    id_visitante = visitante.id_visitante,
                    fecha_entrada = DateTime.Now
                });

                _context.SaveChanges();
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                ViewBag.Compañias = new SelectList(_context.Companias, "id_compañia", "nombre");
                ViewBag.Empleados = new SelectList(_context.Empleados, "id_empleado", "nombre");
                ModelState.AddModelError(string.Empty, "Error al registrar el visitante.");
                return View(visitante);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}