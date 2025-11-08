using HelpDesk.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.ViewModels
{
    public class TicketAutorizacionViewModel
    {
        public int IdTicketAutorizacion { get; set; }

        public int IdTicket { get; set; }

        public int IdArea { get; set; }
        public string NombreArea { get; set; } = string.Empty;
        public int IdEstadoAutorizacion { get; set; }
        public string NombreEstado { get; set; } = string.Empty;

        public string? Observacion { get; set; }

        public DateTime FechaModificacion { get; set; } = DateTime.Now;

    }
}
