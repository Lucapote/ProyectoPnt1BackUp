using Microsoft.EntityFrameworkCore;

namespace ProyectoPNT1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ProyectoPNT1.Models.Persona> Persona { get; set; }
        public DbSet<ProyectoPNT1.Models.Tecnico> Tecnico { get; set; }
        public DbSet<ProyectoPNT1.Models.Horario> Horario { get; set; }
    }
}
