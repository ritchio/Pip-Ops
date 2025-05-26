using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Ticket.Interfaces
{
    public interface ITicketService
    {
        Task<bool> GuardarTicket(TicketViewModel ticketViewModel);
        Task<TicketViewModel> ObtenerPorId(int idTicket);
        int ObtenerMaximoTicketPorUsuario(string idUsuario);
        List<TicketsCantidadesPorEstadoViewModel> ObtenerCantidadesPorEstado(CatUsuarioViewModel usuario);
        Task<bool> Validar(int idTicket, string idUsuario);
        bool Asignar(int idTicket, string idUsuario);

        Task<bool> CambiarEstado(int idTicket, int nuevoEstado, string idUsuario);
    }
}
