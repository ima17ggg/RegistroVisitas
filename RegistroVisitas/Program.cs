using Microsoft.EntityFrameworkCore;
using RegistroVisitas.Data;

var builder = WebApplication.CreateBuilder(args);

// conexión a la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MVC para controladores/vistas
builder.Services.AddControllersWithViews();

//permitir inicio de sesion
builder.Services.AddSession();

builder.WebHost.UseUrls("http://0.0.0.0:5000");

var app = builder.Build();

app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseStaticFiles();

// Rutas MVC (Home/Index como página raíz)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
