using HelpDesk.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.ViewModels
{
    public class CatAreaViewModel
    {
        public int IdArea{ get; set; }
        public required string Nombre { get; set; }
        public bool? Activo { get; set; }
        [Display(Name = "Estado")]
        public string? Estado { get; set; }
    }
}
