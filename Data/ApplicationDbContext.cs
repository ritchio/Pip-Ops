using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace HelpDesk.Data;

public class ApplicationDbContext : IdentityDbContext<Cat_Usuario,IdentityRole,string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Cat_Plaza> Cat_Plaza { get; set; }
    public DbSet<Cat_Area> Cat_Area { get; set; } = default!;
    //public DbSet<Cat_Rol> Cat_Rol{ get; set; } = default!;
    //public DbSet<Rel_UsuarioArea> Rel_UsuarioArea { get; set; } = default!;  // Añadimos DbSet para la relación
    //public DbSet<Rel_UsuarioRol> Rel_UsuarioRol { get; set; } = default!;  // Añadimos DbSet para la relación
    //public DbSet<Rel_UsuarioPermiso> Rel_UsuarioPermiso { get; set; } = default!;  // Añadimos DbSet para la relación

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configuración para Rel_UsuarioArea
        //builder.Entity<Rel_UsuarioArea>()
        //    .HasKey(x => new { x.IdUsuario, x.IdArea });  // Definimos la clave primaria compuesta

        //builder.Entity<Rel_UsuarioArea>()
        //    .HasOne(x => x.Usuario)
        //    .WithMany(u => u.UsuarioAreas)
        //    .HasForeignKey(x => x.IdUsuario)  // Relación con Usuario
        //    .OnDelete(DeleteBehavior.Restrict);  // Evita el borrado en cascada de Usuario

        //builder.Entity<Rel_UsuarioArea>()
        //    .HasOne(x => x.Area)
        //    .WithMany(a => a.UsuarioAreas)
        //    .HasForeignKey(x => x.IdArea)  // Relación con Area
        //    .OnDelete(DeleteBehavior.Restrict);  // Evita el borrado en cascada de Area
    }


}
