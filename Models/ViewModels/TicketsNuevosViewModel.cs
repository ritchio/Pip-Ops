namespace HelpDesk.Models.ViewModels
{
    public class TicketsNuevosViewModel
    {
        public int? IdTicket { get; set; }
        public int? Turno { get; set; }
        public string? Solicitante { get; set; } = string.Empty;

        public string? Plaza { get; set; } = string.Empty;

        public string? Area { get; set; } = string.Empty;

        public string? Asunto { get; set; } = string.Empty;

        public string? TipoSoporte { get; set; } = string.Empty;

        public DateTime? FechaSolicitud { get; set; }

    }
}
