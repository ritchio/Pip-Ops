using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.Entities
{
    [Table("Doc_Ticket")]
    public class Doc_Ticket
    {
        [Key]
        public int IdTicket { get; set; }
        public int? Turno { get; set; }
        public int? Numero { get; set; }
        public int? IdTipoSoporte { get; set; }

        public int? IdModuloClasificacion { get; set; }
        public int IdEstadoTicket{ get; set; }

        [Required]
        [StringLength(450)]
        public string IdUsuario { get; set; } = string.Empty;

        [StringLength(50)]
        public string? CorreoElectronico { get; set; }

        [StringLength(10)]
        public string? Telefono { get; set; }

        [StringLength(140)]
        public string? Asunto { get; set; }

        [StringLength(1000)]
        public string? Cuerpo { get; set; }

        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaCerrado { get; set; }
        public DateTime? FechaValidado { get; set; }

        // Relaciones de navegación
        [ForeignKey(nameof(IdTipoSoporte))]
        public virtual Cat_TipoSoporte? TipoSoporte { get; set; }

        [ForeignKey(nameof(IdModuloClasificacion))]
        public virtual Cat_ModuloClasificacion? ModuloClasificacion { get; set; }

        [ForeignKey(nameof(IdEstadoTicket))]
        public virtual Cat_EstadoTicket? EstadoTicket { get; set; }

       
        public virtual ICollection<Doc_TicketAdjunto>? Adjuntos { get; set; } = new List<Doc_TicketAdjunto>();
        public virtual ICollection<Doc_TicketAutorizacion>? Autorizacion { get; set; }
        public virtual ICollection<Doc_TicketAsignacion>? Asignacion { get; set; }
        public virtual ICollection<Doc_TicketRespuesta>? Respuesta { get; set; }


        [ForeignKey(nameof(IdUsuario))]
        public virtual Cat_Usuario? Usuario { get; set; }
    }
}
