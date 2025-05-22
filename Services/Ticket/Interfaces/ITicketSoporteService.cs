using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Ticket.Interfaces
{
    public interface ITicketSoporteService
    {
        List<TicketsNuevosViewModel> ObtenerNuevos();
        List<TicketsPorAutorizarViewModel> ObtenerPorAutorizar();
        List<TicketsRechazadosViewModel> ObtenerRechazados();
        List<TicketsAbiertosViewModel> ObtenerAbiertos();
        List<TicketsPendientesViewModel> ObtenerPendientes();
        List<TicketsCerradosViewModel> ObtenerCerrados();
        List<TicketsValidadosViewModel> ObtenerValidados();
        List<TicketsRespuestaViewModel> ObtenerRespuesta();
    }
}
