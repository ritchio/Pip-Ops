using HelpDesk.Models.Entities;

namespace HelpDesk.Models.ViewModels
{
    public class HistorialViewModel
    {
        public List<LogAccesoViewModel> Accesos { get; set; }
        public List<LogTicketActividadViewModel> TicketsActividades { get; set; }
    }
}
