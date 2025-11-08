using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.ViewModels
{
    public class CatTipoSoporteViewModel
    {
        public int IdTipoSoporte { get; set; }

        public string Nombre { get; set; }

        public bool Activo { get; set; } = true;
        public string Estado { get; set; }
    }
}
