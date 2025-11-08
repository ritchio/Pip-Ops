using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.Entities
{
    [Table("Doc_TicketRespuestaAdjunto")]
    public class Doc_TicketRespuestaAdjunto
    {
        [Key]
        public int IdTicketRespuestaAdjunto { get; set; }

        [Required]
        public int IdTicketRespuesta { get; set; }

        [StringLength(100)]
        public string? Nombre { get; set; }

        [StringLength(64)]
        public string? Hash { get; set; }

        [StringLength(50)]
        public string? TipoMime { get; set; }

        public byte[]? Archivo { get; set; }

        // Relación de navegación
        [ForeignKey(nameof(IdTicketRespuesta))]
        public virtual Doc_TicketRespuesta? TicketRespuesta { get; set; }
    }
}
