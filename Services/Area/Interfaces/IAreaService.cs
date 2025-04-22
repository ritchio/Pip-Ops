using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Area.Interfaces
{
    public interface IAreaService
    {
        Task<List<CatAreaViewModel>> ObtenerAreasAsync();
        Task<CatAreaViewModel> ObtenerAreaPorIdAsync(int id);
        Task<bool> CrearAreaAsync(Cat_Area Area);
        Task<bool> ActualizarAreaAsync(Cat_Area Area);
        Task<bool> CambiarEstadoAreaAsync(int id);
    }
}
