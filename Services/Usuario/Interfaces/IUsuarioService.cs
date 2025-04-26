using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Usuario.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<AspNetUsersViewModel>> ObtenerUsuarios();
        Task<AspNetUsersViewModel> ObtenerUsuarioPorId(string id);
        Task<RespuestaGenerica<AspNetUsersViewModel>> Crear(AspNetUsersViewModel model);
        Task<AspNetUsersViewModel> Modificar(AspNetUsersViewModel model);
    }
}
