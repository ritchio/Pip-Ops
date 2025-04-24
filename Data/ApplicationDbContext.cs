using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace HelpDesk.Data;

public class ApplicationDbContext
    : IdentityDbContext<Cat_Usuario, IdentityRole, string,
                        IdentityUserClaim<string>,
                        Rel_UsuarioRol,
                        IdentityUserLogin<string>,
                        IdentityRoleClaim<string>,
                        IdentityUserToken<string>>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Cat_Plaza> Cat_Plaza { get; set; }
    public DbSet<Cat_Area> Cat_Area { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        builder.Entity<Rel_UsuarioRol>()
           .HasKey(ur => new { ur.UserId, ur.RoleId });

        builder.Entity<Rel_UsuarioRol>()
            .HasOne(ur => ur.Usuario)
            .WithMany(u => u.RolesUsuario)
            .HasForeignKey(ur => ur.UserId);

        builder.Entity<Rel_UsuarioRol>()
            .HasOne(ur => ur.Rol)
            .WithMany()
            .HasForeignKey(ur => ur.RoleId);

        //// Configuración para incluir los roles relacionados
        //builder.Entity<Cat_Usuario>()
        //    .HasMany(u => u.UserRoles)
        //    .WithOne()
        //    .HasForeignKey(ur => ur.UserId)
        //    .IsRequired();

        //builder.Entity<IdentityRole>()
        //    .HasMany<IdentityUserRole<string>>()
        //    .WithOne()
        //    .HasForeignKey(ur => ur.RoleId)
        //    .IsRequired();
        //// Configuración para la relación entre Cat_Usuario y Roles
        //builder.Entity<Cat_Usuario>()
        //    .HasMany(u => u.Roles)
        //    .WithMany()
        //    .UsingEntity<IdentityUserRole<string>>(
        //        j => j.HasOne<IdentityRole>().WithMany().HasForeignKey(ur => ur.RoleId),
        //        j => j.HasOne<Cat_Usuario>().WithMany().HasForeignKey(ur => ur.UserId)
        //    );
    }


}
