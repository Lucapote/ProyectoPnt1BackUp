using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoPNT1.Models;

namespace ProyectoPNT1.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        #region OnModelCreating cambiamos un par de nombres
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Establecer nombres para los identity stores

            builder.Entity<IdentityUser<int>>().ToTable("Usuarios");
            builder.Entity<IdentityUserRole<int>>().ToTable("PersonasRoles");
            builder.Entity<IdentityRole<int>>().ToTable("Roles");
            #endregion
        }
        #endregion

        public DbSet<ProyectoPNT1.Models.Persona> Persona { get; set; }
        public DbSet<ProyectoPNT1.Models.Tecnico> Tecnico { get; set; }
        public DbSet<ProyectoPNT1.Models.Horario> Horario { get; set; }
        public DbSet<ProyectoPNT1.Models.Rol> Roles { get; set; }
    }
}
