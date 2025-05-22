using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Ticket.Interfaces
{
    public interface ITicketUsuarioService
    {
        List<TicketsNuevosViewModel> ObtenerNuevos(string idUsuario);
        List<TicketsPorAutorizarViewModel> ObtenerPorAutorizar(string idUsuario);
        List<TicketsRechazadosViewModel> ObtenerRechazados(string idUsuario);
        List<TicketsAbiertosViewModel> ObtenerAbiertos(CatUsuarioViewModel usuario);
        List<TicketsPendientesViewModel> ObtenerPendientes(CatUsuarioViewModel usuario);
        List<TicketsCerradosViewModel> ObtenerCerrados(CatUsuarioViewModel usuario);
        List<TicketsValidadosViewModel> ObtenerValidados(CatUsuarioViewModel usuario);
        List<TicketsRespuestaViewModel> ObtenerRespuesta(CatUsuarioViewModel usuario);
    }
}
