using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Autorizacion.Interfaces
{
    public interface IAutorizacionService
    {
        Task<List<CatAutorizacionViewModel>> ObtenerAutorizacionesAsync();
        Task<CatAutorizacionViewModel> ObtenerAutorizacionPorIdAsync(int idAutorizacion);
        Task<bool> CrearAutorizacionAsync(CatAutorizacionViewModel autorizacion);
        Task<bool> ActualizarAutorizacionAsync(CatAutorizacionViewModel autorizacion);
        //Task<List<CatModuloClasificacionViewModel>> ObtenerModulosClasificacionAsync();
        //Task<List<CatAreaViewModel>> ObtenerAreasAsync();
    }
}
