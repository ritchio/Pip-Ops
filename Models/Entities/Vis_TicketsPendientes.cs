using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Models.Entities
{
    [Keyless]
    public class Vis_TicketsPendientes
    {
        public int? IdTicket { get; set; }
        public int? Turno { get; set; }
        public int? Numero { get; set; }
        public string? Solicitante { get; set; } = string.Empty;
        public string? NombreAgente { get; set; } = string.Empty;

        public string? Plaza { get; set; } = string.Empty;

        public string? Area { get; set; } = string.Empty;

        public string? Asunto { get; set; } = string.Empty;

        public string? TipoSoporte { get; set; } = string.Empty;

        public DateTime? FechaSolicitud { get; set; }

        public string? IdUsuario { get; set; }
        public string? IdUsuarioAgente { get; set; }

    }

}
