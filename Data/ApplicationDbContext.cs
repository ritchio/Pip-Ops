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
    public DbSet<Cat_EstadoAutorizacion> Cat_EstadoAutorizacion { get; set; } = default!;
    public DbSet<Cat_EstadoTicket> Cat_EstadoTicket { get; set; } = default!;
    public DbSet<Cat_ModuloClasificacion> Cat_ModuloClasificacion { get; set; } = default!;
    public DbSet<Cat_TipoSoporte> Cat_TipoSoporte { get; set; } = default!;
    public DbSet<Cat_Usuario> Cat_Usuario { get; set; } = default!;
    public DbSet<Cfg_Opcion> Cfg_Opcion { get; set; }
    public DbSet<Doc_Ticket> Doc_Ticket { get; set; }
    public DbSet<Doc_TicketAdjunto> Doc_TicketAdjunto { get; set; }
    public DbSet<Doc_TicketAsignacion> Doc_TicketAsignacion { get; set; }
    public DbSet<Doc_TicketAutorizacion> Doc_TicketAutorizacion { get; set; }
    public DbSet<Doc_TicketRespuesta> Doc_TicketRespuesta { get; set; }
    public DbSet<Doc_TicketRespuestaAdjunto> Doc_TicketRespuestaAdjunto { get; set; }
    public DbSet<Log_Acceso> Log_Acceso { get; set; }
    public DbSet<Log_TicketActividad> Log_TicketActividad { get; set; }
    public DbSet<Rel_Autorizacion_AreaAutoriza> Rel_Autorizacion_AreaAutoriza { get; set; }
    public DbSet<Rel_ModuloClasificacion_Area> Rel_ModuloClasificacion_Area { get; set; }
    public DbSet<Rel_UsuarioArea> Rel_UsuarioArea { get; set; }

    #endregion [ Entidades ]
    protected override void OnModelCreating(ModelBuilder builder)
    {
        #region [ Identity ]

        base.OnModelCreating(builder);
        builder.Entity<Cat_Area>()
            .HasKey(ca => ca.IdArea);

        builder.Entity<Cat_Usuario>()
            .HasKey(cu => cu.Id);

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

        #endregion [ Identity ]

        #region [ Modulo/Clasificaciones ]

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

        #endregion [ Modulo/Clasificaciones ]

        #region [ Autorizaciones ]

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
            .HasKey(r => new { r.IdAutorizacion, r.IdArea });

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

        #endregion [ Autorizaciones ]

        #region [ Tickets ]
        builder.Entity<Doc_Ticket>()
            .HasOne(ti => ti.EstadoTicket)
            .WithMany(et => et.Tickets)
            .HasForeignKey(ti => ti.IdEstadoTicket)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Doc_Ticket>()
            .HasOne(mc => mc.ModuloClasificacion)
            .WithMany(ti => ti.Tickets)
            .HasForeignKey(mc => mc.IdModuloClasificacion)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Doc_Ticket>()
            .HasOne(ts => ts.TipoSoporte)
            .WithMany(ti => ti.Tickets)
            .HasForeignKey(ts => ts.IdTipoSoporte)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Doc_Ticket>()
            .HasOne(ti => ti.Usuario)
            .WithMany(cu => cu.Tickets)
            .HasForeignKey(ti => ti.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

        //ticket adjunto
        builder.Entity<Doc_TicketAdjunto>()
            .HasOne(ta => ta.Ticket)
            .WithMany(ti => ti.Adjuntos)
            .HasForeignKey(ta => ta.IdTicket)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Doc_TicketAdjunto>()
            .HasOne(ta => ta.Usuario)
            .WithMany(cu => cu.TicketAdjuntos)
            .HasForeignKey(ta => ta.IdUsuario);

        //ticket autorizacion
        builder.Entity<Doc_TicketAutorizacion>()
            .HasOne(ta => ta.Ticket)
            .WithMany(ti => ti.Autorizacion)
            .HasForeignKey(ta => ta.IdTicket)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Doc_TicketAutorizacion>()
            .HasOne(te=>te.EstadoAutorizacion)
            .WithMany(ea => ea.TicketAutorizacion)
            .HasForeignKey(te => te.IdEstadoAutorizacion)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Doc_TicketAutorizacion>()
            .HasOne(a => a.Area)
            .WithMany(aa => aa.TicketAutorizacion)
            .HasForeignKey(a => a.IdArea)
            .OnDelete(DeleteBehavior.Cascade);

        //ticket asignacion
        builder.Entity<Doc_TicketAsignacion>()
            .HasOne(ta => ta.Ticket)
            .WithMany(ti => ti.Asignacion)
            .HasForeignKey(ta => ta.IdTicket)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Doc_TicketAsignacion>()
            .HasOne(ta => ta.Usuario)
            .WithMany(cu => cu.TicketAsignacion)
            .HasForeignKey(ta => ta.IdUsuario);

        //ticket respuesta
        builder.Entity<Doc_TicketRespuesta>()
            .HasOne(ta => ta.Ticket)
            .WithMany(ti => ti.Respuesta)
            .HasForeignKey(ta => ta.IdTicket)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Doc_TicketRespuesta>()
            .HasOne(ta => ta.Usuario)
            .WithMany(cu => cu.TicketRespuesta)
            .HasForeignKey(ta => ta.IdUsuario);

        //ticket respuesta adjunto
        builder.Entity<Doc_TicketRespuestaAdjunto>()
            .HasOne(ta => ta.TicketRespuesta)
            .WithMany(ti => ti.Adjuntos)
            .HasForeignKey(ta => ta.IdTicketRespuesta)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion [ Tickets ]

    }


}
