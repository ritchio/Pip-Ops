using HelpDesk.Models.Entities;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Area.Interfaces
{
    public interface IAreaService
    {
        Task<List<CatAreaViewModel>> ObtenerAreasAsync();
        Task<CatAreaViewModel> ObtenerAreaPorIdAsync(int id);
        Task<List<CatAreaViewModel>> ObtenerAreasPorPlazaAsync(int idPlaza);
        Task<RespuestaGenerica<bool>> CrearAreaAsync(Cat_Area Area);
        Task<RespuestaGenerica<bool>> ActualizarAreaAsync(Cat_Area Area);
        Task<bool> CambiarEstadoAreaAsync(int id);
    }
}
