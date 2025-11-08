using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Autorizacion.Interfaces
{
    public interface IAutorizacionService
    {
        Task<List<CatAutorizacionViewModel>> ObtenerAutorizacionesAsync();
        Task<CatAutorizacionViewModel> ObtenerAutorizacionPorIdAsync(int idAutorizacion);
        Task<CatAutorizacionViewModel> ObtenerAutorizacionPorModuloAsync(int idModulo);
        Task<RespuestaGenerica<bool>> CrearAutorizacionAsync(CatAutorizacionViewModel autorizacion);
        Task<RespuestaGenerica<bool>> ActualizarAutorizacionAsync(CatAutorizacionViewModel autorizacion);
        //Task<List<CatModuloClasificacionViewModel>> ObtenerModulosClasificacionAsync();
        //Task<List<CatAreaViewModel>> ObtenerAreasAsync();
    }
}
