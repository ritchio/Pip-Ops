using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;
using System.Security.Claims;

namespace HelpDesk.Services.Usuario.Interfaces
{
    public interface IUsuarioService
    {
        Task<CatUsuarioViewModel> ObtenerUsuarioActual(ClaimsPrincipal claim);
        Task<List<CatUsuarioViewModel>> ObtenerUsuarios();
        Task<RespuestaGenerica<CatUsuarioViewModel>> ObtenerUsuarioPorId(string id);
        Task<RespuestaGenerica<CatUsuarioViewModel>> Crear(CatUsuarioViewModel model);
        Task<CatUsuarioViewModel> Modificar(CatUsuarioViewModel model);
        Task<RespuestaGenerica<Dictionary<string, string>>> CambiarEstado(string idUsuario);
        Task<List<PermisoViewModel>> ObtenerPermisosDeUsuario(string idUsuario);
        Task<RespuestaGenerica<string>> ActualizarPermisoUsuario(string idUsuario, string permiso, bool asignar);
        Task<RespuestaGenerica<string>> CambiarContrasenia(ClaimsPrincipal claim, string nuevaContrasena);
    }
}
