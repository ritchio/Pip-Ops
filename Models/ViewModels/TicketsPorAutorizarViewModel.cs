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

        public int? AutorizacionesPendientes { get; set; }
        public int? AutorizacionesTotales { get; set; }

        public string? TipoSoporte { get; set; } = string.Empty;

        public DateTime? FechaSolicitud { get; set; }

    }
}
