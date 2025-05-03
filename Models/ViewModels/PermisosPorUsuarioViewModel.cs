using System.Reflection.Metadata.Ecma335;

namespace HelpDesk.Models.ViewModels
{
    public class PermisosPorUsuarioViewModel
    {
        public string IdUsuario { get; set; }

        public List<PermisoViewModel> Permisos { get; set; }
    }
}
