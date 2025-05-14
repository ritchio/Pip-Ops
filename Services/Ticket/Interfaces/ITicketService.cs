using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Ticket.Interfaces
{
    public interface ITicketService
    {
        Task<bool> GuardarTicket(TicketViewModel ticketViewModel);
    }
}
