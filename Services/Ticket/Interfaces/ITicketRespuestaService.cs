using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Ticket.Interfaces
{
    public interface ITicketRespuestaService
    {
        Task<bool> Guardar(TicketRespuestaViewModel respuesta);
    }
}
