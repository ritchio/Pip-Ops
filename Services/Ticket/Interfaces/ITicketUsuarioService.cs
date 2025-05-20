using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Ticket.Interfaces
{
    public interface ITicketUsuarioService
    {
        List<TicketsNuevosViewModel> ObtenerNuevos(string idUsuario);
        List<TicketsPorAutorizarViewModel> ObtenerPorAutorizar(string idUsuario);
        List<TicketsRechazadosViewModel> ObtenerRechazados(string idUsuario);

    }
}
