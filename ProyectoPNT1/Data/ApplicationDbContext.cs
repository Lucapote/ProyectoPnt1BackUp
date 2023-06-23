using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProyectoPNT1.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ProyectoPNT1.Models.Persona> Persona { get; set; }
        public DbSet<ProyectoPNT1.Models.Tecnico> Tecnico { get; set; }
        public DbSet<ProyectoPNT1.Models.Horario> Horario { get; set; }
    }
}
