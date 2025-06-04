using HelpDesk.Models.Entities;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Plaza.Interfaces
{
    public interface IPlazaService
    {
        Task<List<CatPlazaViewModel>> ObtenerPlazasAsync();
        Task<CatPlazaViewModel> ObtenerPlazaPorIdAsync(int id);
        Task<RespuestaGenerica<bool>> CrearPlazaAsync(Cat_Plaza plaza);
        Task<RespuestaGenerica<bool>> ActualizarPlazaAsync(Cat_Plaza plaza);
        Task<bool> CambiarEstadoPlazaAsync(int id);
    }
}
