using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RegistroVisitas.Data;
using RegistroVisitas.Models;
using RegistroVisitas.Models.ViewModels;

namespace RegistroVisitas.Controllers
{
    public class VigilanciaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VigilanciaController(ApplicationDbContext context)
        {
            _context = context;
        }

        //tabla de visitas activas
        public IActionResult Vigilancia_visita()
        {
            if (HttpContext.Session.GetString("Vigilancia") != "true")
                return RedirectToAction("Login", "Auth");

            var hoy = DateTime.Today;
            var mañana = hoy.AddDays(1);

            var visitantesActivos = _context.Visitas
                .Include(v => v.Visitante)
                    .ThenInclude(v => v.Compañia)
                .Include(v => v.Visitante)
                    .ThenInclude(v => v.Empleado)
                .Where(v => 
                    v.fecha_entrada >= hoy && 
                    v.fecha_entrada < mañana)
                .Select(v => new VigilanciaViewModel
                {
                    id_visita = v.id_visita,
                    nombre = v.Visitante.nombre,
                    apellido_p = v.Visitante.apellido_p,
                    apellido_m = v.Visitante.apellido_m,
                    asunto = v.Visitante.asunto,
                    compania = v.Visitante.Compañia != null
                        ? v.Visitante.Compañia.nombre
                        : "N/A",
                    foto = v.Visitante.foto,
                    empleado = v.Visitante.Empleado != null
                        ? v.Visitante.Empleado.nombre
                        : "N/A",
                    fecha_entrada = v.fecha_entrada,
                    fecha_salida = v.fecha_salida
                })
                .ToList();

            return View(visitantesActivos);
        }

        //modificar salida de visita
        [HttpPost]
        public IActionResult RegistrarSalida(int id_visita)
        {
            var visita = _context.Visitas.FirstOrDefault(v => v.id_visita == id_visita);
            if (visita != null && visita.fecha_salida == null)
            {
                visita.fecha_salida = DateTime.Now;
                _context.SaveChanges();
            }
            return RedirectToAction("Vigilancia_visita");
        }

        // Map this action to /Gafetes/GenerarGafete
        [HttpGet("/Gafetes/GenerarGafete")]
        public IActionResult generarGafete(int visitaID)
        {
            var visita = _context.Visitas
                .Include(v => v.Visitante)
                .ThenInclude(v => v.Compañia)
                .FirstOrDefault(v => v.id_visita == visitaID && v.fecha_salida == null);

            if (visita == null)
            {
                return NotFound("Visita no encontrada o ya ha salido.");
            }

            var QRgenerator = new QRCodeGenerator();
            var QRdata = QRgenerator.CreateQrCode(
                visita.id_visita.ToString(),
                QRCodeGenerator.ECCLevel.Q);

            var QRcode = new PngByteQRCode(QRdata);
            byte[] qrBytes = QRcode.GetGraphic(20);

            var model = new VigilanciaViewModel
            {
                nombre = visita.Visitante.nombre,
                apellido_p = visita.Visitante.apellido_p,
                apellido_m = visita.Visitante.apellido_m,
                compania = visita.Visitante.Compañia?.nombre ?? "N/A",
                foto = visita.Visitante.foto,
                QRCodeData = qrBytes
            };

            // Use explicit view path to match Views\Gafetes\gafete.cshtml
            return View("~/Views/Gafetes/gafete.cshtml", model);
        }

        public IActionResult ver_visitas_pasadas()
        {
            return RedirectToAction("VisitasPasadas", "visitasPasadas");
        }

        public IActionResult Salir()
        {
            return RedirectToAction("Index");
        }
    }
}
