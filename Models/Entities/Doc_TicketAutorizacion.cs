using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.Entities
{
    public class Doc_TicketAutorizacion
    {
        [Key]
        public int IdTicketAutorizacion { get; set; }

        [Required]
        public int IdTicket { get; set; }

        [Required]
        public int? IdArea { get; set; }

        [Required]
        public int? IdEstadoAutorizacion { get; set; }

        public string? Observacion { get; set; }

        public DateTime? FechaModificacion { get; set; } = DateTime.Now;

        // Relaciones de navegación
        [ForeignKey(nameof(IdTicket))]
        public virtual Doc_Ticket? Ticket { get; set; }

        [ForeignKey(nameof(IdArea))]
        public virtual Cat_Area? Area { get; set; }

        [ForeignKey(nameof(IdEstadoAutorizacion))]
        public virtual Cat_EstadoAutorizacion? EstadoAutorizacion { get; set; }
    }
}
