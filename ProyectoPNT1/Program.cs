using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProyectoPNT1.Data;
using ProyectoPNT1.Models;
using ProyectoPNT1.Recursos;
using System.Data;

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

#region crear usuario para cuando se inicializa la aplicacion y asignar los roles
// Agregar el usuario por defecto y asignar los roles
using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<Persona>>();
    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<Rol>>();
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Verificar si el usuario predeterminado ya existe
    var usuarioPorDefecto = await userManager.FindByNameAsync(UXD.Email);
    if (usuarioPorDefecto == null)
    {
        // Crear el usuario predeterminado
        usuarioPorDefecto = new Persona
        {
            UserName = UXD.Email,
            Email = UXD.Email,
            Nombre = UXD.Nombre,
            Apellido = UXD.Apellido
        };
        await userManager.CreateAsync(usuarioPorDefecto, UXD.Password); // Establecer la contraseña deseada
    }

    // Asegurarse de que la base de datos esté creada
    dbContext.Database.EnsureCreated();

    // Crear los roles si no existen
    foreach (var nombreRol in Enum.GetNames(typeof(RolesEnum)))
    {
        if (!await roleManager.RoleExistsAsync(nombreRol))
        {
            await roleManager.CreateAsync(new Rol { Name = nombreRol });
        }
    }

    // Asignar los roles al usuario por defecto
    foreach (var roleName in Enum.GetNames(typeof(RolesEnum)))
    {
        await userManager.AddToRoleAsync(usuarioPorDefecto, roleName);
    }
}

#endregion

#region


#endregion

app.Run();
