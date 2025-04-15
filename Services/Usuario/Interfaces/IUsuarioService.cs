using HelpDesk.Data;
using HelpDesk.Models.ViewModels;

namespace HelpDesk.Services.Usuario.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<AspNetUsersViewModel>> ObtenerUsuarios();
        Task<AspNetUsersViewModel> ObtenerUsuarioPorId(string id);
        Task<AspNetUsersViewModel> Modificar(AspNetUsersViewModel model);
    }
}
