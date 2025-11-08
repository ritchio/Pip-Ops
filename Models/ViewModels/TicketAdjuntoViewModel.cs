using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.ViewModels
{
    public class TicketAdjuntoViewModel
    {
        public int IdTickeAdjunto { get; set; }

        [Required]
        public int IdTicket { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string TipoMime { get; set; } = string.Empty;

        public byte[] Archivo { get; set; } = Array.Empty<byte>();

        public DateTime Fecha { get; set; }

        [Required]
        public string IdUsuario { get; set; } = string.Empty;

        [MaxLength(64)]
        public string? Hash { get; set; }

        // Para despliegue
        public string? NombreUsuario { get; set; }
    }
}
