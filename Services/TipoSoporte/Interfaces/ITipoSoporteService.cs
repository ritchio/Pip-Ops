using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.TipoSoporte.Interfaces
{
    public interface ITipoSoporteService
    {
        Task<List<CatTipoSoporteViewModel>> ObtenerTiposSoporteAsync();

    }
}
