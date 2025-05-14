using HelpDesk.Models.Entities;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.ModuloClasificacion.Interfaces
{
    public interface IModuloClasificacionService
    {
        Task<List<CatModuloClasificacionViewModel>> ObtenerModulosClasificacionesAsync();
        Task<CatModuloClasificacionViewModel> ObtenerModuloClasificacionPorIdAsync(int idModulo);
        Task<List<CatModuloClasificacionViewModel>> ObtenerModulosClasificacionesPorTipoSoporteAsync(int idTipoSoporte);
        Task<List<CatTipoSoporteViewModel>> ObtenerTiposSoporteAsync();
        Task<bool> CrearModuloClasificacionAsync(CatModuloClasificacionViewModel moduloClasificacion);
        RespuestaGenerica<CatModuloClasificacionViewModel> CambiarEstado(int idModulo);
        CatModuloClasificacionViewModel ModificarModuloClasificacion(CatModuloClasificacionViewModel moduloClasificacion);
    }
}
