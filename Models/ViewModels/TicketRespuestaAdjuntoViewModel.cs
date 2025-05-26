using HelpDesk.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.ViewModels
{
    public class TicketRespuestaAdjuntoViewModel
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

    }
}
