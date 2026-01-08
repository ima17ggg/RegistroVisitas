// Ensure you have installed the Microsoft.EntityFrameworkCore NuGet package in your project.
// You can do this by running the following command in the Package Manager Console:
// Install-Package Microsoft.EntityFrameworkCore

namespace RegistroVisitas.Data
{
    using Microsoft.EntityFrameworkCore;
    using RegistroVisitas.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Visitante> Visitantes { get; set; }
        public DbSet<Visita> Visitas { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Compañia> Companias { get; set; }
        public DbSet<Etiqueta> Etiquetas { get; set; }
    }
}
