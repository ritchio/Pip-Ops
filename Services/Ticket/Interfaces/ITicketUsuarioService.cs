using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Ticket.Interfaces
{
    public interface ITicketUsuarioService
    {
        List<TicketsNuevosViewModel> ObtenerNuevos(CatUsuarioViewModel usuario);
        List<TicketsPorAutorizarViewModel> ObtenerPorAutorizar(CatUsuarioViewModel usuario);
        List<TicketsRechazadosViewModel> ObtenerRechazados(CatUsuarioViewModel usuario);
        List<TicketsAbiertosViewModel> ObtenerAbiertos(CatUsuarioViewModel usuario);
        List<TicketsPendientesViewModel> ObtenerPendientes(CatUsuarioViewModel usuario);
        List<TicketsCerradosViewModel> ObtenerCerrados(CatUsuarioViewModel usuario);
        List<TicketsValidadosViewModel> ObtenerValidados(CatUsuarioViewModel usuario);
        List<TicketsRespuestaViewModel> ObtenerRespuesta(CatUsuarioViewModel usuario);
    }
}
