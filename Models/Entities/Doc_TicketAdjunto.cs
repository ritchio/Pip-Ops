using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.Entities
{
    [Table("Doc_TicketAdjunto")]
    public class Doc_TicketAdjunto
    {
        [Key]
        public int IdTicketAdjunto { get; set; }

        [Required]
        public int IdTicket { get; set; }

        [StringLength(100)]
        public string? Nombre { get; set; }

        [StringLength(64)]
        public string? Hash { get; set; }

        [StringLength(50)]
        public string? TipoMime { get; set; }

        public byte[]? Archivo { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [StringLength(450)]
        public string IdUsuario { get; set; } = string.Empty;

        // Relaciones de navegación
        [ForeignKey(nameof(IdTicket))]
        public virtual Doc_Ticket? Ticket { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public virtual Cat_Usuario? Usuario { get; set; }
    }
}
