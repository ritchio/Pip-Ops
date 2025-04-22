using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Plaza.Interfaces
{
    public interface IPlazaService
    {
        Task<List<CatPlazaViewModel>> ObtenerPlazasAsync();
        Task<CatPlazaViewModel> ObtenerPlazaPorIdAsync(int id);
        Task<bool> CrearPlazaAsync(Cat_Plaza plaza);
        Task<bool> ActualizarPlazaAsync(Cat_Plaza plaza);
        Task<bool> CambiarEstadoPlazaAsync(int id);
    }
}
