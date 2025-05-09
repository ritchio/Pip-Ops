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

    #region [ Entidades ]
    public DbSet<Cat_Plaza> Cat_Plaza { get; set; }
    public DbSet<Cat_Area> Cat_Area { get; set; } = default!;
    public DbSet<Cat_Usuario> Cat_usuario { get; set; } = default!;
    public DbSet<Cfg_Opcion> Cfg_Opcion { get; set; }
    public DbSet<Log_Acceso> Log_Acceso { get; set; }
    public DbSet<Log_TicketActividad> Log_TicketActividad { get; set; }
    public DbSet<Rel_UsuarioArea> Rel_UsuarioArea { get; set; }
    
    #endregion [ Entidades ]
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Cat_Usuario>()
            .HasKey(cu => cu.Id );

        builder.Entity<Cat_Usuario>()
        .Property(u => u.NumeroUsuario)
        .HasDefaultValueSql("NEXT VALUE FOR NumeroUsuario_Seq");

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

        builder.Entity<Rel_UsuarioArea>()
            .HasKey(rua => new { rua.IdUsuario, rua.IdArea });
       
    }


}
