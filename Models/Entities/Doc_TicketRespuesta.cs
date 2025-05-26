using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.Entities
{
    [Table("Doc_TicketRespuesta")]
    public class Doc_TicketRespuesta
    {
        [Key]
        public int IdTicketRespuesta { get; set; }

        [Required]
        public int IdTicket { get; set; }

        public string? Mensaje { get; set; }

        [Required]
        [StringLength(450)]
        public string IdUsuario { get; set; } = string.Empty;

        public bool DelSolicitante { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;

        // Relaciones de navegación
        [ForeignKey(nameof(IdTicket))]
        public virtual Doc_Ticket? Ticket { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public virtual Cat_Usuario? Usuario { get; set; }

        public virtual ICollection<Doc_TicketRespuestaAdjunto>? Adjuntos { get; set; } = [];
    }
}
