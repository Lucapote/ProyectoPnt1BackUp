using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoPNT1.Data;
using ProyectoPNT1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region ConexionBaseDeDatos
builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer("name = DefaultConnection"));
#endregion

#region identity
//asi si los dos son asignados
builder.Services.AddIdentity<Persona, Rol>().AddEntityFrameworkStores<ApplicationDbContext>();

//asi es si queremos dejar uno asignado y otro por defecto
//builder.Services.AddIdentity<Usuario, IdentityRole<int>>().AddEntityFrameworkStores<ApplicationDbContext>();
#endregion

#region manejo restricciones
//aca redireccionamos a donde querramos que se necesiten logear o se le deniegue el acceso
builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, opciones =>
{
    opciones.LoginPath = "/Cuentas/IniciarSesion";
    opciones.AccessDeniedPath = "/Cuentas/IniciarSesion";
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
#region autenticacion con identity
//agregamos autenticacion
app.UseAuthentication();
//antes de la autorizacion
#endregion
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
