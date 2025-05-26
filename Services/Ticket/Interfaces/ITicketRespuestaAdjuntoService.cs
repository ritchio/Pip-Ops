using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Ticket.Interfaces
{
    public interface ITicketRespuestaAdjuntoService
    {
        Task<bool> GuardarAdjunto(TicketRespuestaViewModel ticketViewModel);
        Task<bool> EliminarAdjunto(int idAdjunto);
        Task<Doc_TicketRespuestaAdjunto> ObtenerPorHash(string hash);

        Task<List<TicketRespuestaAdjuntoViewModel>> ObtenerAdjuntosPorTicketId(int idTicket);
        Task<TicketRespuestaAdjuntoViewModel> ObtenerAdjuntoPorId(int idAdjunto);
    }
}