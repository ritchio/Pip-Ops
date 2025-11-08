using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.ViewModels
{
    public class TicketsPorAutorizarViewModel
    {
        public int? IdTicket { get; set; }
        public int? Turno { get; set; }
        public int? Numero { get; set; }
        public string? Solicitante { get; set; } = string.Empty;

        public string? Plaza { get; set; } = string.Empty;

        public string? Area { get; set; } = string.Empty;

        public string? Asunto { get; set; } = string.Empty;
        public string? Cuerpo { get; set; } = string.Empty;

        public int? AutorizacionesCompletadas { get; set; }
        public int? AutorizacionesTotales { get; set; }
        [NotMapped]
        public int AutorizacionesPendientes => (AutorizacionesTotales ?? 0) - (AutorizacionesCompletadas ?? 0);


        public string? TipoSoporte { get; set; } = string.Empty;

        public DateTime? FechaSolicitud { get; set; }

    }
}
