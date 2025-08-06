namespace HelpDesk.Models.ViewModels
{
    public class TicketsBuscarViewModel
    {
        public int IdTicket { get; set; }
        public int Turno { get; set; } 

        public int Numero { get; set; } 

        public string? Solicitante { get; set; }

        public string? Plaza { get; set; }

        public string? Area { get; set; }

        public string? Asunto { get; set; }

        public string? Cuerpo { get; set; }

        public string? TipoSoporte { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public string? IdUsuario { get; set; }

        public string? Agente { get; set; }
        public string? Estatus { get; set; }
        

    }
}
