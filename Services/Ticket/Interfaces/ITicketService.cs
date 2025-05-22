using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Ticket.Interfaces
{
    public interface ITicketService
    {
        Task<bool> GuardarTicket(TicketViewModel ticketViewModel);
        Task<TicketViewModel> ObtenerPorId(int idTicket);
        int ObtenerMaximoTicketPorUsuario(string idUsuario);
        Task<bool> Validar(int idTicket, string idUsuario);
    }
}
