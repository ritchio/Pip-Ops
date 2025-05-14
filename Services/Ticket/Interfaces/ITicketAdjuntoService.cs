using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Ticket.Interfaces
{
    public interface ITicketAdjuntoService
    {
        Task<bool> GuardarAdjunto(TicketViewModel ticketViewModel);
        Task<bool> EliminarAdjunto(int idAdjunto);
        Task<List<TicketAdjuntoViewModel>> ObtenerAdjuntosPorTicketId(int idTicket);
        Task<TicketAdjuntoViewModel> ObtenerAdjuntoPorId(int idAdjunto);
    }
}
