using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.Entities
{
    [Table("Doc_TicketAsignacion")]
    public class Doc_TicketAsignacion
    {
        [Key]
        public int IdTicketAsignacion { get; set; }

        [Required]
        public int IdTicket { get; set; }

        [Required]
        [StringLength(450)]
        public string IdUsuario { get; set; } = string.Empty;

        public int? Turno { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaAsignacion { get; set; } = DateTime.Now;

        public DateTime? FechaDesasignacion { get; set; }

        // Relaciones de navegación
        [ForeignKey(nameof(IdTicket))]
        public virtual Doc_Ticket Ticket { get; set; } = default!;

        [ForeignKey(nameof(IdUsuario))]
        public virtual Cat_Usuario Usuario { get; set; } = default!;
    }
}
