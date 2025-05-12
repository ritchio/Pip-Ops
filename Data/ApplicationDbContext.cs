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
    public DbSet<Cat_Autorizacion> Cat_Autorizacion { get; set; } = default!;
    public DbSet<Cat_ModuloClasificacion> Cat_ModuloClasificacion { get; set; } = default!;
    public DbSet<Cat_TipoSoporte> Cat_TipoSoporte { get; set; } = default!;
    public DbSet<Cat_Usuario> Cat_Usuario { get; set; } = default!;
    public DbSet<Cfg_Opcion> Cfg_Opcion { get; set; }
    public DbSet<Log_Acceso> Log_Acceso { get; set; }
    public DbSet<Log_TicketActividad> Log_TicketActividad { get; set; }
    public DbSet<Rel_Autorizacion_AreaAutoriza> Rel_Autorizacion_AreaAutoriza { get; set; }
    public DbSet<Rel_ModuloClasificacion_Area> Rel_ModuloClasificacion_Area { get; set; }
    public DbSet<Rel_UsuarioArea> Rel_UsuarioArea { get; set; }
    
    #endregion [ Entidades ]
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Cat_Area>()
            .HasKey(ca => ca.IdArea);

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
        
        // modulo clasificacion
        
        builder.Entity<Cat_ModuloClasificacion>()
            .HasOne(mc => mc.TipoSoporte)
            .WithMany(ts => ts.ModulosClasificacion)
            .HasForeignKey(mc => mc.IdTipoSoporte)
            .OnDelete(DeleteBehavior.Restrict); 

        builder.Entity<Rel_ModuloClasificacion_Area>()
            .HasKey(r => new { r.IdModuloClasificacion, r.IdArea });

        builder.Entity<Rel_ModuloClasificacion_Area>()
            .HasOne(r => r.ModuloClasificacion)
            .WithMany(mc => mc.RelacionAreas)
            .HasForeignKey(r => r.IdModuloClasificacion)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.Entity<Rel_ModuloClasificacion_Area>()
            .HasOne(r => r.Area)
            .WithMany(a => a.ModuloClasificacionAreas)
            .HasForeignKey(r => r.IdArea)
            .OnDelete(DeleteBehavior.Cascade); 
        
        // autorizacion
        
        builder.Entity<Cat_Autorizacion>()
            .HasOne(au => au.AreaPertenece)
            .WithMany(aa => aa.Autorizaciones)
            .HasForeignKey(au => au.IdArea)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Cat_Autorizacion>()
            .HasOne(au => au.ModuloClasificacion)
            .WithMany(aa => aa.Autorizaciones)
            .HasForeignKey(au => au.IdModuloClasificacion)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Rel_Autorizacion_AreaAutoriza>()
            .HasKey(r => new { r.IdAutorizacion,r.IdArea });

        builder.Entity<Rel_Autorizacion_AreaAutoriza>()
            .HasOne(r => r.Autorizacion)
            .WithMany(mc => mc.RelacionAreasAutorizan)
            .HasForeignKey(r => r.IdAutorizacion)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.Entity<Rel_Autorizacion_AreaAutoriza>()
            .HasOne(r => r.Area)
            .WithMany(a => a.AutorizacionAreas)
            .HasForeignKey(r => r.IdArea)
            .OnDelete(DeleteBehavior.Cascade);


    }


}
