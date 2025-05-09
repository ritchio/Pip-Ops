using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Historial.Interfaces
{
    public interface IHistorialService
    {
        Task<List<LogAccesoViewModel>> ObtenerAccesosAsync();
        Task<List<LogTicketActividadViewModel>> ObtenerTicketsActividadesAsync();

    }
}
