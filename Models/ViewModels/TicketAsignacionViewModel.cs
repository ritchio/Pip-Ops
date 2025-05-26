using HelpDesk.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.ViewModels
{
    public class TicketAsignacionViewModel
    {
        public int IdTicketAsignacion { get; set; }
        public int IdTicket { get; set; }
        public string IdUsuario { get; set; } = string.Empty;
        public int? Turno { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaAsignacion { get; set; } = DateTime.Now;

        public DateTime? FechaDesasignacion { get; set; }

    }
}
