using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;
using System.Threading.Tasks;

namespace HelpDesk.Services.Ticket.Interfaces
{
    public interface ITicketService
    {
        Task<List<TicketViewModel>> Busqueda(string? busqueda);
        Task<bool> GuardarTicket(TicketViewModel ticketViewModel, CatUsuarioViewModel usuario);
        Task<TicketViewModel> ObtenerPorId(int idTicket);
        Task<List<Cat_EstadoTicket>> ObtenerEstadosTicket();
        int ObtenerMaximoTicketPorUsuario(string idUsuario);
        List<TicketsCantidadesPorEstadoViewModel> ObtenerCantidadesPorEstado(CatUsuarioViewModel usuario);
        Task<bool> Validar(int idTicket, string idUsuario);
        Task<bool> Asignar(int idTicket, string idUsuario, string idUsuarioActual);

        Task<bool> CambiarEstado(int idTicket, int nuevoEstado, string idUsuario);

        Task<List<ReporteAgenteViewModel>> AtendidosPorAgente(DateTime fechaInicio, DateTime fechaFinal, string? idUsuario);
        Task<List<ReporteDetalladoViewModel>> ReporteDetallado(DateTime fechaInicio, DateTime fechaFinal);
    }
}
