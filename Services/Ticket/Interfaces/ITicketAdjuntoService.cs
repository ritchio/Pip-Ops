using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Ticket.Interfaces
{
    public interface ITicketAdjuntoService
    {
        Task<bool> GuardarAdjunto(TicketViewModel ticketViewModel);
        Task<bool> EliminarAdjunto(int idAdjunto);
        Task<Doc_TicketAdjunto> ObtenerPorHash(string hash);
        Task<List<TicketAdjuntoViewModel>> ObtenerAdjuntosPorTicketId(int idTicket);
        Task<TicketAdjuntoViewModel> ObtenerAdjuntoPorId(int idAdjunto);
    }
}
