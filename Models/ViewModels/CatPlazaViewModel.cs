using HelpDesk.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.ViewModels
{
    public class CatPlazaViewModel
    {
        public int IdPlaza { get; set; }
        public string Nombre { get; set; }
        public bool? Activo { get; set; }
        [Display(Name = "Estatus")]
        public string? Estado { get; set; }
    }
}
